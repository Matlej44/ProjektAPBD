namespace Projekt.Entity;

public class Payment
{
    public int PaymentId { get; set; }
    public int ContractId { get; set; }
    public DateTime Date { get; set; }
    public int ClientId { get; set; }
    public decimal Amount { get; set; }
    
    public Contract Contract { get; set; }
    public Client Client { get; set; }
}