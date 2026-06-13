namespace Projekt.Entity;

public class Software
{
    public int SoftwareId { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public int SoftwareTypeId { get; set; }
    public int ClientId { get; set; }
    public int SoftwareVersionId { get; set; }
    
    
    
    
    public ICollection<Contract> Contracts { get; set; }
    public SoftwareType SoftwareType { get; set; }
    public ICollection<SoftwareVersion> SoftwareVersions { get; set; }
    public SoftwareVersion CurrentVersion { get; set; }
    public Client Client { get; set; }
    
    public ICollection<Discount> Discounts { get; set; }
    public ICollection<SubscriptionOffer> SubscriptionOffers { get; set; }
}