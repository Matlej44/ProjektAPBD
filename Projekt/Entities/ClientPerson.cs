using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Data;

namespace Projekt.Entity;

public class ClientPerson : Client, ISoftDelete
{
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public string Pesel { get; set; }
    [NotMapped] public bool IsSoftDeleted =false;
    
    public void SoftDelete()
    {
        Name = "Deleted";
        Surname = "Deleted";
        Email = "deleted@deleted.com";
        Pesel = "0000000000";
        PhoneNumber = "000-000-000";
        IsSoftDeleted = true;
    }
}