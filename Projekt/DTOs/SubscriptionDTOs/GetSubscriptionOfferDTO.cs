namespace Projekt.DTOs.SubscriptionDTOs;

public class GetSubscriptionOfferDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string RenewalMonts { get; set; }
    public decimal Price { get; set; }
    public string SoftwareName { get; set; }
}