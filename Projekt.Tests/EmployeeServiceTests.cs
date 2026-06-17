using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Projekt.Data;
using Projekt.DTOs.EmployeeDTOs;
using Projekt.Entity;
using Projekt.Exceptions;
using Projekt.Services;
using Xunit.Abstractions;

namespace DefaultNamespace;

public class EmployeeServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EmployeeServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        
        return new AppDbContext(options);
    }

    private Mock<IPasswordHasher<Employee>> GetMockPasswordHasher()
    {
        var MockPasswordHasher = new Mock<IPasswordHasher<Employee>>();
        return MockPasswordHasher;
    }
    [Fact]
    public async Task CreateEmployeeLoginTaken_ThrowsBadRequestException()
    {
        await using var context = GetDbContext();
        var mock = GetMockPasswordHasher();
        mock.Setup(x => x.HashPassword(It.IsAny<Employee>(), It.IsAny<string>())).Returns("hashedPassword");
        var service = new EmployeeService(mock.Object, context);
        var dto = new CreateEmployeeDTO
        {
            Login = "test",
            Password = "111111111",
            Role = "admin"
        };
        await service.CreateEmployeeAsync(dto);
        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateEmployeeAsync(dto));
    }
    [Fact]
    public async Task CreateEmployee_ValidData_AddsEmployeeToDatabase()
    {
        await using var context = GetDbContext();
        var mock = GetMockPasswordHasher();
        mock.Setup(x => x.HashPassword(It.IsAny<Employee>(), It.IsAny<string>())).Returns("hashedPassword");
        var service = new EmployeeService(mock.Object, context);
        var dto = new CreateEmployeeDTO
        {
            Login = "test",
            Password = "111111111",
            Role = "admin"
        };
        await service.CreateEmployeeAsync(dto);
        var employee = context.Employees.FirstOrDefault(e => e.Login == dto.Login);
        Assert.NotNull(employee);
        Assert.Equal(dto.Role, employee.Role);
        Assert.Equal("hashedPassword", employee.PasswordHash);
        Assert.Equal(dto.Login, employee.Login);
    }
    [Fact]
    public async Task CreateEmployee_PasswordTooShort_ThrowsBadRequestException()
    {
        await using var context = GetDbContext();
        var mock = GetMockPasswordHasher();
        mock.Setup(x => x.HashPassword(It.IsAny<Employee>(), It.IsAny<string>())).Returns("hashedPassword");
        var service = new EmployeeService(mock.Object, context);
        var dto = new CreateEmployeeDTO
        {
            Login = "test",
            Password = "1",
            Role = "admin"
        };
        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateEmployeeAsync(dto));
    }
    [Fact]
    public async Task CreateEmployee_InvalidRole_ThrowsBadRequestException()
    {
        await using var context = GetDbContext();
        var mock = GetMockPasswordHasher();
        mock.Setup(x => x.HashPassword(It.IsAny<Employee>(), It.IsAny<string>())).Returns("hashedPassword");
        var service = new EmployeeService(mock.Object, context);
        var dto = new CreateEmployeeDTO
        {
            Login = "test",
            Password = "111111111",
            Role = "invalidRole"
        };
        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateEmployeeAsync(dto));
    }
    
}