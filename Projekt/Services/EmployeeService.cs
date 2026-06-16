using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.EmployeeDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly AppDbContext _context;
    
    public EmployeeService(IPasswordHasher<Employee> passwordHasher, AppDbContext context)
    {
        _passwordHasher = passwordHasher;
        _context = context;
    }

    public async Task<List<GetEmployeeDTO>> GetAllEmployeesAsync()
    {
        var result = await _context.Employees.Select(x => new GetEmployeeDTO
        {
            Login = x.Login,
            Id = x.EmployeeId,
            Role = x.Role
        }).ToListAsync();
        return result;
    }

    public async Task<GetEmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO employeeDto)
    {
        var employeeExists = await _context.Employees.AnyAsync(x => x.Login == employeeDto.Login);
        if (employeeExists)
            throw new BadRequestException("Taki logi jest już zajety");
        
        var newEmployee = new Employee
        {
            Login = employeeDto.Login,
            Role = employeeDto.Role
        };
        var passwordHashed = _passwordHasher.HashPassword(newEmployee, employeeDto.Password);
        newEmployee.PasswordHash = passwordHashed;
        var employee = await _context.Employees.AddAsync(newEmployee);
        await _context.SaveChangesAsync();
        return new GetEmployeeDTO
        {
            Id = employee.Entity.EmployeeId,
            Login = employee.Entity.Login,
            Role = employee.Entity.Role
        };
    }

    public async Task<GetEmployeeDTO> GetEmployeeByIdAsync(int id)
    {
        var res = await _context.Employees.FindAsync(id);
        if (res == null)
            throw new NotFoundException("Nie znaleziono pracownika o takim id");
        return new GetEmployeeDTO
        {
            Id = res.EmployeeId,
            Login = res.Login,
            Role = res.Role
        };
    }

    public async Task<GetEmployeeDTO> GetEmployeeByLoginAsync(string login)
    {
        var res = await _context.Employees.FirstOrDefaultAsync(x => x.Login==login);
        if (res == null)
            throw new NotFoundException("Nie znaleziono pracownika o takim loginie");
        return new GetEmployeeDTO
        {
            Id = res.EmployeeId,
            Login = res.Login,
            Role = res.Role
        };
    }
}