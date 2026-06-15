namespace Projekt.DTOs.SubscriptionDTOs;

public class GetSubscriptionDTO
{
    public int SubsriptionId { get; set; }
    public string clientEmail { get; set; }
    public string clientPhoneNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string SubscriptionName { get; set; }
    public int LeftDaysOfSubscription { get; set; }
    //If client overpaid
    public decimal? Retuns { get; set; }
    
}