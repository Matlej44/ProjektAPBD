namespace Projekt.Entity;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}