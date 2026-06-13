namespace Projekt.Entity;

public class Contract
{
    public int ContractId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    //Przedział czasowy jest wyliczany na podstwie tej daty utworzenia nie ma potrzeby trzymać jej w bazie danych
    public DateTime CreatedAt { get; set; }
    public int ClientId { get; set; }
    //Maximum is 3 years each year worth additional 1000 PLN
    public int AdditionalSupportYears { get; set; }
    
    public int SoftwareVersionId { get; set; }
    

    //Na podstawie tej relacji można wyliczyć liste bo musimy tylko pamietać o podstawowej wersji
    public SoftwareVersion SoftwareVersion { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public Client Client { get; set; }
    
}