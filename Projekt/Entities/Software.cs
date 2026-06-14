using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Entity;

public class Software
{
    public int SoftwareId { get; set; }
    public string Name { get; set; }
    public int SoftwareTypeId { get; set; }
    
    [NotMapped]
    public SoftwareVersion? CurrentVersion => SoftwareVersions.OrderByDescending(x => x.ReleaseDate).FirstOrDefault();
    
    
    
    
    public ICollection<Contract> Contracts { get; set; }
    public SoftwareType SoftwareType { get; set; }
    public ICollection<SoftwareVersion> SoftwareVersions { get; set; }
    
    public ICollection<Discount> Discounts { get; set; }
    public ICollection<SubscriptionOffer> SubscriptionOffers { get; set; }
}