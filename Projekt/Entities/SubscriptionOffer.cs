namespace Projekt.Entity;

public class SubscriptionOffer
{
    public int SubscriptionOfferId { get; set; }
    public string Name { get; set; }
    //In months
    public int RenewalPeriod { get; set; }
    public decimal Price { get; set; }
    public int SoftwareId { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public Software Software { get; set; }
}