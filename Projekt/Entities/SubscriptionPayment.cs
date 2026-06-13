namespace Projekt.Entity;

public class SubscriptionPayment
{
    public int SubscriptionPaymentId { get; set; }
    public int SubscriptionId { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public decimal Amount { get; set; }
    
    public Subscription Subscription { get; set; }
}