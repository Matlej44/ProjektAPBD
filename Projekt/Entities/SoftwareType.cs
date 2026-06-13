namespace Projekt.Entity;

public class SoftwareType
{
    public int SoftwareTypeId { get; set; }
    public string Name { get; set; }
    
    public ICollection<Software> Software { get; set; }
}