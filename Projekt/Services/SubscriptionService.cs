using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.SubscriptionDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class SubscriptionService : ISubscrptionService
{
    private readonly AppDbContext _context;

    public SubscriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetSubscriptionDTO> GetSubscription(int subscriptionId)
    {
        var subscription = await _context.Subscriptions.Include(x => x.SubscriptionOffer)
            .Include(x => x.Client).FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId);
        if (subscription == null)
            throw new NotFoundException("Subscrybcja o takim id nie istnieje");
        var dto = new GetSubscriptionDTO
        {
            clientPhoneNumber = subscription.Client.PhoneNumber,
            clientEmail = subscription.Client.Email,
            EndDate = subscription.EndDate,
            StartDate = subscription.StartDate,
            SubscriptionName = subscription.SubscriptionOffer.Name,
            SubsriptionId = subscription.SubscriptionId,
            LeftDaysOfSubscription = (int)Math.Round((subscription.EndDate - DateTime.Now).TotalDays > 0
                ? (subscription.EndDate - DateTime.Now).TotalDays
                : 0)
        };
        return dto;
    }

    public async Task<GetSubscriptionDTO> CreateSubscrption(int offerId, CreateSubscriptionDTO subscription)
    {
        var subscriptionOffer = await _context.SubscriptionOffers.FindAsync(offerId);
        if (subscriptionOffer == null)
            throw new NotFoundException("Nie znaleziono oferty o takim id");
        var client = await _context.Clients.FindAsync(subscription.ClientId);
        if (client == null)
            throw new NotFoundException("Nie znaleziono klienta o takim id");
        var startDate = DateTime.Now;
        var endDate = startDate.AddMonths(subscriptionOffer.RenewalPeriod);
        var paymentNeeded = subscriptionOffer.Price * (1 - await FindBiggestDiscount(subscriptionOffer.SoftwareId));
        if (await IsReturningClient(client.ClientId))
        {
            paymentNeeded *= 0.95m;
        }

        decimal? returnal = null;
        if (subscription.PaymentAmount < paymentNeeded)
            throw new BadRequestException("Wpłacono za mało pieniedzy");
        if (subscription.PaymentAmount > paymentNeeded)
        {
            returnal = subscription.PaymentAmount - paymentNeeded;
            subscription.PaymentAmount = paymentNeeded;
        }

        var subscriptionNew = new Subscription
        {
            ClientId = client.ClientId,
            SubscriptionOfferId = subscriptionOffer.SubscriptionOfferId,
            StartDate = startDate,
            EndDate = endDate,
        };
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var entityEntry = await _context.Subscriptions.AddAsync(subscriptionNew);
            await _context.SaveChangesAsync();
            var payment = new SubscriptionPayment
            {
                Amount = subscription.PaymentAmount,
                PaymentDate = DateTime.Now,
                SubscriptionId = entityEntry.Entity.SubscriptionId
            };
            var paymentEntry = await _context.SubscriptionPayments.AddAsync(payment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            var dto = new GetSubscriptionDTO
            {
                clientEmail = client.Email,
                clientPhoneNumber = client.PhoneNumber,
                EndDate = subscriptionNew.EndDate,
                StartDate = subscriptionNew.StartDate,
                SubscriptionName = subscriptionOffer.Name,
                SubsriptionId = subscriptionNew.SubscriptionId,
                LeftDaysOfSubscription = (int)Math.Round((subscriptionNew.EndDate - DateTime.Now).TotalDays > 0
                    ? (subscriptionNew.EndDate - DateTime.Now).TotalDays
                    : 0),
                Retuns = returnal
            };
            return dto;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<string> CreatePayment(int subscriptionId, PaymentDTO payment)
    {
        var subscription = await _context.Subscriptions.Include(x => x.SubscriptionOffer).FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId);
        if (subscription == null)
            throw new NotFoundException("Nie znaleziono subskrybcji o takim id");
        if (subscription.EndDate < DateTime.Now)
            throw new BadRequestException("Subskrybcja już wygasła");
        if (subscription.EndDate.AddDays(-30) > DateTime.Now)
            throw new BadRequestException("Nie jesteś w okresie opłacenia za subskrybcje");
        //Według tekstu w trakcie odnowienia stosujemy najwiekszą aktyną subskypcje
        //nie subsykrupcje aktywną w trakcie pierwszego zakupu
        var newPrice = subscription.SubscriptionOffer.Price * (1 - await FindBiggestDiscount(subscription.SubscriptionOffer.SoftwareId))*0.95m;
        if (payment.Amount < newPrice)
            throw new BadRequestException($"Wpłacono za mało pieniedzy brakuje {newPrice - payment.Amount}");
        var returnal = payment.Amount - newPrice;
        if (returnal > 0)
            payment.Amount = newPrice;
        
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var paymentNew = new SubscriptionPayment
            {
                Amount = payment.Amount,
                PaymentDate = DateTime.Now,
                SubscriptionId = subscriptionId
            };
            var entityEntry = await _context.SubscriptionPayments.AddAsync(paymentNew);
            subscription.EndDate = subscription.EndDate.AddMonths(subscription.SubscriptionOffer.RenewalPeriod);
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return returnal > 0 ? $"Zapłacono za dużo zwrócono nadwyżke w wysokości {returnal} zł" : "Zapłacono za kolejny okres subskrypcji";
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<List<GetSubscriptionOfferDTO>> GetSubscriptionOffer(int softwareId)
    {
        var software = await _context.Softwares.FindAsync(softwareId);
        if (software == null)
            throw new NotFoundException("Nie znaleziono oprogramowania o takim id");
        var offers = await _context.SubscriptionOffers
            .Where(x => x.SoftwareId == softwareId).Select(x => new GetSubscriptionOfferDTO
            {
                Id = x.SubscriptionOfferId,
                Name = x.Name,
                Price = x.Price,
                RenewalMonts = $"subskrybcja trwa {x.RenewalPeriod} miesięcy",
                SoftwareName = x.Software.Name
            }).ToListAsync();
        return offers;
    }

    private async Task<bool> IsReturningClient(int clientId)
    {
        var contracts = await _context.Contracts.Where(x => x.ClientId == clientId).ToListAsync();
        var subscriptions = await _context.Subscriptions.Where(x => x.ClientId == clientId).ToListAsync();
        return contracts.Count != 0 || subscriptions.Count != 0;
    }

    private async Task<decimal> FindBiggestDiscount(int softwareId)
    {
        var date = DateTime.Now;
        var discounts = await _context.Discounts
            .Include(d => d.Software)
            .Where(d => d.Software.Any(s => s.SoftwareId == softwareId))
            .ToListAsync(); // pobierz do pamięci PRZED filtrowaniem dat

        var best = discounts
            .Where(d =>
                (!d.IsRepetitive && d.StartDate <= date && d.EndDate >= date)
                ||
                (d.IsRepetitive &&
                 new DateTime(date.Year, d.StartDate.Month, d.StartDate.Day) <= date &&
                 new DateTime(date.Year, d.EndDate.Month, d.EndDate.Day) >= date)
            )
            .Select(d => (decimal?)d.DiscountPercent)
            .Max();

        return best ?? 0m;
    }
}