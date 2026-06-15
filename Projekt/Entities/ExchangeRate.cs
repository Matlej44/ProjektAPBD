namespace Projekt.Entity;

public class ExchangeRate
{
    public DateOnly Date { get; set; }
    public string Base { get; set; }
    public string Quote { get; set; }
    public decimal Rate { get; set; }
}