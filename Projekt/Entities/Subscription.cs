namespace Projekt.Entity;

public class Subscription
{
    public int SubscriptionId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Client Client { get; set; }
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
}