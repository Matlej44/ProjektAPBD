using System.Globalization;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Projekt.Data;
using Projekt.DTOs.RevenueDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class RevenueService : IRevenueService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public RevenueService(AppDbContext context, IMemoryCache cache, HttpClient httpClient)
    {
        _context = context;
        _cache = cache;
        _httpClient = httpClient;
    }

    public async Task<GetRevenueDTO> GetCurrentRevenueAsync(string? currency, string? softwareName)
    {
        var rate = await GetRate(currency ?? "PLN");
        
        
        //To jest nasz cały dorobek za wszyskie lata, Jako że nie usuwamy kontraktów ani ich nie dezaktywujemy
        //Kontrakt raz zapłacony jest dożywotni ale tylko na określone wersje oprogramowania.
        //Zdobywam wszystkie dane dlatego że są one potem w pamieci mapowane
        var contracts = await _context.Payments
            .Where(x => x.Contract.IsActive&&(x.Contract.SoftwareVersion.Softwares.Name==softwareName||softwareName==null)).ToListAsync();
        //Każde wpłacenie pieniedzy w subsciption Payments można od razu traktować jako przychód gdyż jest to gwarancja
        //Że te pieniądze przedłużyły lub aktywowały komuś jego subskrybcje
        var subscriptions = await _context.SubscriptionPayments
            .Where(x => x.Subscription.SubscriptionOffer.Software.Name==softwareName||softwareName==null).ToListAsync();

        var contractIncome = contracts.Sum(x => x.Amount);
        var subscriptionIncome = subscriptions.Sum(x => x.Amount);
        var income = contractIncome + subscriptionIncome;

        var contractYearlyIncome = contracts.Where(x => x.Date.Year == DateTime.Now.Year).Sum(x => x.Amount);
        var subscriptionYearlyIncome =
            subscriptions.Where(x => x.PaymentDate.Year == DateTime.Now.Year).Sum(x => x.Amount);
        var yearlyIncome = contractYearlyIncome + subscriptionYearlyIncome;

        var dictContracts = new Dictionary<string, decimal>();
        var dictSubscriptions = new Dictionary<string, decimal>();
        var dictYearly = new Dictionary<string, decimal>();
        //Monthly income
        for (int i = 1; i < 13; i++)
        {
            var monthName = CultureInfo.GetCultureInfo("en-en").DateTimeFormat.GetMonthName(i);
            var contractsInMonth = contracts
                .Where(x => x.Date.Month == i && x.Date.Year == DateTime.Now.Year)
                .Sum(x => x.Amount);
            var subscriptionsInMonth = subscriptions
                .Where(x => x.PaymentDate.Month == i && x.PaymentDate.Year == DateTime.Now.Year)
                .Sum(x => x.Amount);

            subscriptionsInMonth *= rate;
            contractsInMonth *= rate;

            dictContracts.Add(monthName, contractsInMonth);
            dictSubscriptions.Add(monthName, subscriptionsInMonth);
            dictYearly.Add(monthName, contractsInMonth + subscriptionsInMonth);
        }

        var getRevenueDto = new GetRevenueDTO
        {
            OverallRevenue = income * rate,
            MonthlyRevenueContracts = dictContracts,
            MonthlyRevenueSubscriptions = dictSubscriptions,
            MonthlyRevenueSoftware = dictYearly,
            OverallRevenueContracts = contractIncome*rate,
            OverallRevenueSubscriptions = subscriptionIncome*rate,
            YearlyRevenue = yearlyIncome*rate,
            YearlyContractRevenue = contractYearlyIncome*rate,
            YearlySubsriptionRevenue = subscriptionYearlyIncome*rate
        };
        return getRevenueDto;
    }

    public async Task<GetPredictedRevenue> GetPredictedRevenueAsync(string? currency, string? softwareName)
    {
        var rate = await GetRate(currency ?? "PLN");
        var currentIncome = await GetCurrentRevenueAsync(currency, softwareName);

        //Wszystkie kontrakty opłacone
        var predictedYearlyIncomeContracts = await _context.Contracts.Where(x => x.CreatedAt.Year == DateTime.Now.Year).SumAsync(x => x.TotalPrice);
        
        //Dla subskrypcji trzeba założyć że klient będzie płacił do końca roku
        var subscriptionsThisYear = await 
            _context.Subscriptions.Include(x => x.SubscriptionOffer).Where(x => x.EndDate.Year == DateTime.Now.Year).ToListAsync();
        var EndOfYear = new DateTime(DateTime.Now.Year, 12, 31);
        var predictedYearlyIncomeSubscriptions = subscriptionsThisYear.Select(x =>
        {
            var price = x.SubscriptionOffer.Price;
            while (x.EndDate < EndOfYear)
            {
                price += x.SubscriptionOffer.Price;
                x.EndDate = x.EndDate.AddMonths(x.SubscriptionOffer.RenewalPeriod);
            }
            return price;
        }).Sum();
        
        var predictedYearlyRevenue = predictedYearlyIncomeSubscriptions+predictedYearlyIncomeContracts;
        var result = new GetPredictedRevenue
        {
            CurrentYearlyRevenue = currentIncome.OverallRevenue*rate,
            PredictedYearlyRevenue = predictedYearlyRevenue*rate,
            CurrentYearlyRevenueContracts = currentIncome.OverallRevenueContracts*rate,
            PredictedYearlyRevenueContracts = predictedYearlyIncomeContracts*rate,
            CurrentYearlyRevenueSubscriptions = currentIncome.OverallRevenueSubscriptions*rate,
            PredictedYearlyRevenueSubscriptions = predictedYearlyIncomeSubscriptions*rate,
            PredictedRevenuePercentage = (predictedYearlyRevenue/currentIncome.OverallRevenue*rate+1)*100,
        };
        
        return result;
    }


    //Our currency convesion uses Frankfurter api which is open source
    //Big thanks to them for this great service
    private async Task<decimal> GetRate( string to)
    {
        to = to.ToUpper();
        if (to.Equals("PLN")) return 1m;

        if (_cache.TryGetValue(to, out decimal cashedRate))
            return cashedRate;
        var url = $"https://api.frankfurter.dev/v2/rate/PLN/{to}";
        var response = await _httpClient.GetAsync(url);
        //Frankfurter zwraca tylko takie błedy
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                throw new BadRequestException("Nie znaleziono kursu dla waluty");
            case HttpStatusCode.NotFound:
                throw new NotFoundException("Nie znaleziono kursu dla waluty");
            case HttpStatusCode.UnprocessableEntity:
                throw new BadRequestException("Nie znaleziono kursu dla waluty");
            default:
                break;
        }

        var responseJson = await response.Content.ReadFromJsonAsync<ExchangeRate>();

        //Można poprawić na resetowanie o 16:15 jako że frankfurter wtedy dostaje nowe dane
        _cache.Set(to, responseJson.Rate, TimeSpan.FromHours(1));
        return responseJson.Rate;
    }
}