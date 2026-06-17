using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Projekt.Data;
using Projekt.DTOs.AuthenticationDTOs;
using Projekt.Entity;
using Projekt.Exceptions;
using Projekt.Services;

namespace DefaultNamespace;

public class AuthServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        
        return new AppDbContext(options);
    }
    [Fact]
    public async Task Login_invalidPassword_ThrowsUnauthorizedException()
    {
        await using var context = GetDbContext();
        var employee = new Employee
        {
            Login = "test",
            PasswordHash = "correctHash",
            Role = "admin"
        };
        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();
        var mockHasher = new Mock<IPasswordHasher<Employee>>();
        var mockConfig = new Mock<IConfiguration>();
        
        mockHasher.Setup(x => x.VerifyHashedPassword(employee, employee.PasswordHash, "wrongPassword")).Returns(PasswordVerificationResult.Failed);
        var authService = new AuthService(context, mockHasher.Object, mockConfig.Object);
        var loginDto = new LoginDTO
        {
            Login = "test",
            Password = "wrongPassword"
        };
        await Assert.ThrowsAsync<UnauthorizedException>(() => authService.Login(loginDto));
    }
}