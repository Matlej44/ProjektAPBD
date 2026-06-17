using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.ClientsDTOs;
using Projekt.Entity;
using Projekt.Exceptions;
using Projekt.Services;

namespace DefaultNamespace;

public class ClientServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddPersonAsync_ValidData_AddsPersonToDatabase()
    {
        var context = GetDbContext();
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
    [Fact]
    public async Task DeletePerson_NonExistingPerson_ThrowsNotFoundException()
    {
        var context = GetDbContext();
        var clientService = new ClientService(context);
        await Assert.ThrowsAsync<NotFoundException>(() => clientService.DeletePersonAsync(999));
    }
}