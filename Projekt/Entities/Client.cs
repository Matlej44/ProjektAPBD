namespace Projekt.Entity;

public abstract class Client
{
    public int ClientId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    
    public ICollection<Contract> Contracts { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Software> Software { get; set; }
}