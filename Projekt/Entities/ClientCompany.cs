using Projekt.Data;

namespace Projekt.Entity;

public class ClientCompany : Client, IBlockDelete
{
    public string CompanyName { get; set; }
    public string Address { get; set; }
    
    public string KRS { get; set; }
}