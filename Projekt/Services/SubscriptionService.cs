using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.SubscriptionDTOs;
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
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
        if (subscription == null)
            throw new NotFoundException("Subscrybcja o takim id nie istnieje");
        var dto = new GetSubscriptionDTO
        {
            clientPhoneNumber = subscription.Client.PhoneNumber,
            clientEmail = subscription.Client.Email,
            EndDate = subscription.EndDate,
            StartDate = subscription.StartDate,
            IsActive = subscription.IsActive,
            SubscriptionName = subscription.SubscriptionOffer.Name,
            SubsriptionId = subscription.SubscriptionId
        };
        return dto;
    }

    public Task<GetSubscriptionDTO> CreateSubscrption(int offerId, CreateSubscriptionDTO subscription)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetSubscriptionOfferDTO>> GetSubscriptionOffer(int softwareId)
    {
        var offers =  await _context.SubscriptionOffers
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
}