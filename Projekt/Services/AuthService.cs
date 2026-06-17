using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projekt.Data;
using Projekt.DTOs.AuthenticationDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IPasswordHasher<Employee> passwordHasher, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<TokenDTO> Login(LoginDTO loginDto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Login == loginDto.Login);
        if (employee == null)
            throw new UnauthorizedException();
        var res = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, loginDto.Password);
        if (res == PasswordVerificationResult.Failed)
            throw new UnauthorizedException();
        if (res == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var hashPassword = _passwordHasher.HashPassword(employee, loginDto.Password);
            employee.PasswordHash = hashPassword;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
        var token = generateTokens(employee);


        return token;
    }

    public TokenDTO generateTokens(Employee employee)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
            new(ClaimTypes.Name, employee.Login),
            new(ClaimTypes.Role, employee.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new TokenDTO { AccessToken = accessToken};
    }
}