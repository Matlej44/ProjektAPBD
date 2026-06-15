using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Projekt.Data;
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

    public async Task<string> GetCurrentRevenueAsync(string? currency)
    {
        var income = 1999m;
        income = await ConvertCurrency(income, currency ?? "PLN");
        return income.ToString();
    }


    //Our currency convesion uses Frankfurter api which is open source
    //Big thanks to them for this great service
    private async Task<decimal> ConvertCurrency(decimal amount, string to)
    {
        to = to.ToUpper();
        if (to.Equals("PLN")) return 1;

        if (_cache.TryGetValue(to, out decimal cashedRate))
            return cashedRate*amount;
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
        return responseJson.Rate*amount;
    }
}