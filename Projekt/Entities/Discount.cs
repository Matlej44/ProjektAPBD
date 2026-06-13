namespace Projekt.Entity;

public class Discount
{
    public int DiscountId { get; set; }
    public decimal DiscountPercent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Name { get; set; }
    
    //It repeats every year on same days
    public bool IsRepetitive { get; set; }
    
    //Discount can be applied to many pieces of software
    public ICollection<Software> Software { get; set; }
}