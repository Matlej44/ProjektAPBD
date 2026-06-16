using Projekt.DTOs.EmployeeDTOs;

namespace Projekt.Services;

public interface IEmployeeService
{
    public Task<List<GetEmployeeDTO>> GetAllEmployeesAsync();
}