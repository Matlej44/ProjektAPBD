using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Projekt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projekt.Entity;
using Projekt.Services;

namespace Projekt;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("docker"));
        });
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IContractService, ContractService>();
        builder.Services.AddScoped<ISubscrptionService, SubscriptionService>();
        builder.Services.AddScoped<IRevenueService, RevenueService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient<RevenueService>();

        builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
        
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (jwtKey == null)
            throw new Exception("Jwt:Key is not set");
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        if (jwtIssuer == null)
            throw new Exception("Jwt:Issuer is not set");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
            };
        });
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("admin"))
            .AddPolicy("User", policy => policy.RequireRole("user", "admin"));
        

        

        var app = builder.Build();
        


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
                options.EnablePersistAuthorization();
            });
        }
        
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}