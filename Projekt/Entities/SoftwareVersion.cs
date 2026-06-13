namespace Projekt.Entity;

public class SoftwareVersion
{
    public int SoftwareVersionId { get; set; }
    public string Version { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    
    public decimal YearlyPrice { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
}