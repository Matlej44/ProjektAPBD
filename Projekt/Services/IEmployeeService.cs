using Projekt.DTOs.EmployeeDTOs;

namespace Projekt.Services;

public interface IEmployeeService
{
    public Task<List<GetEmployeeDTO>> GetAllEmployeesAsync();
    public Task<GetEmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO employeeDto);
    public Task<GetEmployeeDTO> GetEmployeeByIdAsync(int id);
    public Task<GetEmployeeDTO> GetEmployeeByLoginAsync(string login);
}