using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.ClientsDTOs;
using Projekt.Entity;
using Projekt.Exceptions;
using Projekt.Services;
using Xunit.Abstractions;

namespace DefaultNamespace;

public class ClientServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClientServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddPersonAsync_ValidData_AddsPersonToDatabase()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        var dto = new AddPersonDTO
        {
            Email = "Test@test.com", Name = "Jan", Surname = "Kowalski", Pesel = "12345678901",
            PhoneNumber = "123-234-345"
        };
        await clientService.AddPersonAsync(dto);
        
        var personInDb = await context.ClientPersons.FirstOrDefaultAsync(p => p.Pesel == dto.Pesel);
        Assert.NotNull(personInDb);
        Assert.Equal(dto.Email, personInDb.Email);
        Assert.Equal(dto.Name, personInDb.Name);
        Assert.Equal(dto.Surname, personInDb.Surname);
        Assert.Equal(dto.Pesel, personInDb.Pesel);
        Assert.Equal(dto.PhoneNumber, personInDb.PhoneNumber);
    }

    [Theory]
    [InlineData("0000")]
    [InlineData("0000000000000000000")]
    public async Task AddPersonAsync_InvalidData_ThrowsBadRequestException(string pesel)
    {
        await using var context = GetDbContext();
        var service = new ClientService(context);

        var dto = new AddPersonDTO
        {
            Email = "Test@test.com", Name = "Jan", Surname = "Kowalski", Pesel = pesel,
            PhoneNumber = "123-234-345"
        };
        await Assert.ThrowsAsync<BadRequestException>(() => service.AddPersonAsync(dto));
    }
    
    [Fact]
    public async Task DeletePerson_NonExistingPerson_ThrowsNotFoundException()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        await Assert.ThrowsAsync<NotFoundException>(() => clientService.DeletePersonAsync(999));
    }

    [Fact]
    public async Task AddCompanyAsync_ValidData_AddsCompanyToDatabase()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        var dto = new AddCompanyDTO
        {
            Address = "aaaa",
            CompanyName = "aaaaa",
            Email = "test",
            KRS = "0000000001",
            PhoneNumber = "123-234-345"
        };
        await clientService.AddCompanyAsync(dto);
        var companyInDb = await context.ClientCompanies.FirstOrDefaultAsync(c => c.CompanyName == dto.CompanyName);
        Assert.NotNull(companyInDb);
        Assert.Equal(dto.Address, companyInDb.Address);
        Assert.Equal(dto.CompanyName, companyInDb.CompanyName);
        Assert.Equal(dto.Email, companyInDb.Email);
        Assert.Equal(dto.KRS, companyInDb.KRS);
    }

    [Theory]
    [InlineData("0000")]
    [InlineData("0000000000000000000")]
    public async Task AddCompanyAsync_InvalidData_ThrowsBadRequestException(string krs)
    {
        await using var context = GetDbContext();
        var service = new ClientService(context);
        var dto = new AddCompanyDTO
        {
            Address = "aaaa",
            CompanyName = "aaaaa",
            Email = "test",
            KRS = krs
        };
        await Assert.ThrowsAsync<BadRequestException>(() => service.AddCompanyAsync(dto));
    }
    [Fact]
    public async Task TryModifyPersonPesel_ReturnsUnchanged()
    {
        await using var context = GetDbContext();
        var service = new ClientService(context);
        var dto = new AddPersonDTO
        {
            Email = "Test@test.com", Name = "Jan", Surname = "Kowalski", Pesel = "12345678901",
            PhoneNumber = "123-234-345"
        };
        await service.AddPersonAsync(dto);
        var person = await context.ClientPersons.FirstOrDefaultAsync(p => p.Pesel == dto.Pesel);
        person!.Pesel = "12345679901";
        var entityEntry = context.ClientPersons.Update(person);
        await context.SaveChangesAsync();
        Assert.Equal(entityEntry.Entity.Pesel, dto.Pesel);
    }
    [Fact]
    public async Task TryModifyCompanyKRS_ReturnsUnchanged()
    {
        await using var context = GetDbContext();
        var service = new ClientService(context);
        var dto = new AddCompanyDTO
        {
            Address = "aaaa",
            CompanyName = "aaaaa",
            Email = "test",
            KRS = "0000000003",
            PhoneNumber = "123-234-345"
        };
        await service.AddCompanyAsync(dto);
        var company = context.ClientCompanies.FirstOrDefault(c => c.KRS == dto.KRS);
        company.KRS = "0000000001";
        var entityEntry = context.ClientCompanies.Update(company);
        await context.SaveChangesAsync();
        Assert.Equal(entityEntry.Entity.KRS, dto.KRS);
    }
    [Fact]
    public async Task ModifyCompanyAsync_NonExistingCompany_ThrowsNotFoundException()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        await Assert.ThrowsAsync<NotFoundException>(() => clientService.ModifyCompanyAsync(999, new ModifyCompanyDTO()));
    }
    [Fact]
    public async Task ModifyPersonAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        await Assert.ThrowsAsync<NotFoundException>(() => clientService.ModifyPersonAsync(999, new ModifyPersonDTO()));
    }
    [Fact]
    public async Task ModifyCompanyAsync_ValidData_ModifiesCompanyInDatabase()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        var dto = new AddCompanyDTO
        {
            Address = "aaaa",
            CompanyName = "aaaaa",
            Email = "test",
            KRS = "0000000001",
            PhoneNumber = "123-234-345"
        };
        await clientService.AddCompanyAsync(dto);
        var company = context.ClientCompanies.FirstOrDefault(c => c.KRS == dto.KRS);
        var modifyDto = new ModifyCompanyDTO
        {
            Address = "bbbb",
            CompanyName = "bbbbb",
            Email = "test2",
            PhoneNumber = "123-234-346"
        };
        await clientService.ModifyCompanyAsync(company!.ClientId, modifyDto);
        company = context.ClientCompanies.FirstOrDefault(c => c.KRS == dto.KRS);
        Assert.NotNull(company);
        Assert.Equal(modifyDto.Address, company.Address);
        Assert.Equal(modifyDto.CompanyName, company.CompanyName);
        Assert.Equal(modifyDto.Email, company.Email);
        Assert.Equal(modifyDto.PhoneNumber, company.PhoneNumber);
        Assert.Equal(dto.KRS, company.KRS);
    }
    [Fact]
    public async Task ModifyPersonAsync_ValidData_ModifiesPersonInDatabase()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        var person = new ClientPerson
        {
            ClientId = 1,
            Email = "aaa",
            Name = "aa",
            Pesel = "aaaaaaaaa",
            Surname = "aa",
            PhoneNumber = "123-234-345"
        };
        context.ClientPersons.Add(person);
        await context.SaveChangesAsync();
        var modifyDto = new ModifyPersonDTO
        {
            Email = "bbb",
            Name = "bb",
            Surname = "bb",
            PhoneNumber = "123-234-346"
        };
        await clientService.ModifyPersonAsync(person.ClientId, modifyDto);
        person = context.ClientPersons.FirstOrDefault(p => p.Pesel == person.Pesel);
        Assert.NotNull(person);
        Assert.Equal(modifyDto.Email, person.Email);
        Assert.Equal(modifyDto.Name, person.Name);
        Assert.Equal(modifyDto.Surname, person.Surname);
        Assert.Equal(modifyDto.PhoneNumber, person.PhoneNumber);
    }
    [Fact]
    public async Task DeletePerson_DeletesSoftly()
    {
        await using var context = GetDbContext();
        var clientService = new ClientService(context);
        var person = new ClientPerson
        {
            ClientId = 1,
            Email = "aaa",
            Name = "aa",
            Pesel = "aaaaaaaaa",
            Surname = "aa",
            PhoneNumber = "123-234-345"
        };
        context.ClientPersons.Add(person);
        await context.SaveChangesAsync();
        await clientService.DeletePersonAsync(person.ClientId);
        person = context.ClientPersons.FirstOrDefault(p => p.Pesel == person.Pesel);
        var softDelete = await context.ClientPersons.FindAsync(1);
        Assert.Equal("0000000000", person.Pesel);
        Assert.NotNull(softDelete);
    }
    
}