namespace Projekt.Entity;

public class SoftwareSell
{
    public int SoftwareSellId { get; set; }
    public int SoftwareId { get; set; }
    
    public Software Software { get; set; }
}