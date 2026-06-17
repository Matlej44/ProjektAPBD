using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.ContractDTOs;
using Projekt.Exceptions;
using Projekt.Services;

namespace DefaultNamespace;

public class ContractServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        
        return new AppDbContext(options);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    public async Task CreateContractInvalidSupportYears_ThrowsBadRequestException(int supportYears)
    {
        await using var context = GetDbContext();
        var service = new ContractService(context);
        var dto = new AddContractDTO
        {
            ClientId = 1,
            SoftwareId = 1,
            AdditionalSupportYears = supportYears,
            SoftwareVersion = "1.0.0"
        };
        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateContract(dto));
    }
}