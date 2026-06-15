namespace Projekt.Entity;

public class SubscriptionPayment
{
    public int SubscriptionPaymentId { get; set; }
    public int SubscriptionId { get; set; }
    public DateTime PaymentDate { get; set; }
    //Okres odnowienia subskrybcji to 30 dni przed jej wygaśnieciem
    public decimal Amount { get; set; }
    
    public Subscription Subscription { get; set; }
}