using Projekt.DTOs.SubscriptionDTOs;

namespace Projekt.Services;

public interface ISubscrptionService
{
    public Task<GetSubscriptionDTO> GetSubscription(int subscriptionId);
    public Task<GetSubscriptionDTO> CreateSubscrption(int offerId, CreateSubscriptionDTO subscription);
    public Task<List<GetSubscriptionOfferDTO>> GetSubscriptionOffer(int offerId);
    public Task<string> CreatePayment(int subscriptionId, PaymentDTO payment);
}