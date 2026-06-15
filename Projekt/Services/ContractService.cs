using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.ContractDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class ContractService : IContractService
{
    private readonly AppDbContext _context;

    public ContractService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetContractsDTO>> GetAllContractsAsync()
    {
        var listAsync = await _context.Contracts.Select(x => new GetContractsDTO
        {
            AdditionalSupportYears = x.AdditionalSupportYears,
            ClientEmail = x.Client.Email,
            ContractId = x.ContractId,
            CreatedAt = x.CreatedAt,
            EndDate = x.EndDate,
            IsActive = x.IsActive,
            SoftwareVersion = x.SoftwareVersion.Version,
            StartDate = x.StartDate,
            TotalPrice = x.TotalPrice
        }).ToListAsync();
        return listAsync;
    }

    public async Task<GetContractsDTO> CreateContract(AddContractDTO contractDto)
    {
        if (contractDto.StartDate < DateTime.Now.AddDays(30))
            throw new BadRequestException("Nie można ustawić daty przed 30 dniami od aktualnej daty");
        if (contractDto.EndDate < contractDto.StartDate)
            throw new BadRequestException("Data zakonczenia kontraktu nie może być przed datą rozpoczecia");
        if (contractDto.AdditionalSupportYears is < 0 or > 3)
            throw new BadRequestException("Długość przedłużenia wsparcia poza przedziałem");


        if (await HasActiveContract(contractDto.ClientId, contractDto.SoftwareId))
            throw new ConflictException("Już posiadasz aktywny kontrakt dla tego oprogramowania");
        //Jeżeli klient chce aktualną wersję oprogramowania to musimy ją znaleźć
        if (contractDto.SoftwareVersion == "latest")
        {
            var software = await _context.Softwares.FindAsync(contractDto.SoftwareId);
            if (software == null)
                throw new NotFoundException("Nie znaleziono oprogramowania o takim id");
            if (software.CurrentVersion != null)
                contractDto.SoftwareVersion = software.CurrentVersion.Version;
            else
                throw new NotFoundException("Nie znaleziono aktualnej wersji oprogramowania");
        }

        //Jest to unikatowy index
        var softwareVersion = await
            _context.SoftwareVersions.FirstOrDefaultAsync(x =>
                x.Version == contractDto.SoftwareVersion && x.SoftwareId == contractDto.SoftwareId);
        if (softwareVersion == null)
            throw new NotFoundException("Nie znaleziono wersji o takim numerze");
        var client = await _context.Clients.FindAsync(contractDto.ClientId);
        if (client == null)
            throw new NotFoundException("Nie znaleziono klienta o takim id");
        var basePrice = await CalculatePriceAsync(softwareVersion.SoftwareVersionId, contractDto.AdditionalSupportYears);
        if (await IsReturningClient(client.ClientId))
        {
            basePrice *= 0.95m;
        }
        basePrice *= 1 - await FindBiggestDiscount(contractDto.SoftwareId);
        var contract = new Contract
        {
            AdditionalSupportYears = contractDto.AdditionalSupportYears,
            ClientId = client.ClientId,
            CreatedAt = DateTime.Now,
            EndDate = contractDto.EndDate,
            IsActive = false,
            SoftwareVersionId = softwareVersion.SoftwareVersionId,
            StartDate = contractDto.StartDate,
            TotalPrice = basePrice
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        var getContractsDto = new GetContractsDTO
        {
            ContractId = contract.ContractId,
            ClientEmail = client.Email,
            AdditionalSupportYears = contract.AdditionalSupportYears,
            SoftwareVersion = contract.SoftwareVersion.Version,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            TotalPrice = contract.TotalPrice,
            CreatedAt = contract.CreatedAt,
            IsActive = contract.IsActive
        };
        return getContractsDto;
    }

    public async Task<GetContractsDTO> GetContractByIdAsync(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null)
            throw new NotFoundException("Nie znaleziono kontraktu o takim id");
        return new GetContractsDTO
        {
            AdditionalSupportYears = contract.AdditionalSupportYears,
            ClientEmail = contract.Client.Email,
            ContractId = contract.ContractId,
            CreatedAt = contract.CreatedAt,
            EndDate = contract.EndDate,
            IsActive = contract.IsActive,
        };
    }

    public async Task UpdateAllContractsAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var contracts = await _context.Contracts.Where(x => x.IsActive && x.EndDate < DateTime.Now).ToListAsync();
            foreach (var contract in contracts)
            {
                contract.IsActive = false;
            }
            var contractsToActivate = await _context.Contracts.Where( x => !x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).ToListAsync();
            foreach (var contract in contractsToActivate)
            {
                if (await CountTheMoneyDiff(contract.ContractId) == 0)
                {
                    contract.IsActive = true;
                }
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        await transaction.CommitAsync();
    }

    public async Task DeleteContractAsync(int id)
    {
        var findAsync = await _context.Contracts.FindAsync(id);
        if (findAsync == null)
            throw new NotFoundException("Nie znaleziono kontraktu o takim id");
        if (findAsync.IsActive)
            throw new BadRequestException("Nie wolno usunąć aktywnego kontraktu");
        _context.Contracts.Remove(findAsync);
        await _context.SaveChangesAsync();
    }

    public async Task<string> CreatePaymentAsync(int id, AddPaymentDTO paymentDto)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null)
            throw new NotFoundException("Nie znaleziono kontraktu o takim id");
        if(contract.IsActive) 
            throw new BadRequestException("Nie można opłacić aktywnego kontraktu");
        var moneyNeded = await CountTheMoneyDiff(contract.ContractId);
        if (moneyNeded == 0)
            throw new BadRequestException("Kontrakt już opłacony");

        Payment payment;
        switch (moneyNeded - paymentDto.Amount)
        {
            case > 0:
                payment = new Payment
                {
                    Amount = paymentDto.Amount,
                    ClientId = paymentDto.ClientId,
                    ContractId = contract.ContractId,
                    Date = DateTime.Now,
                };
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return $"Zapłacono rate za kontrakt pozostało {moneyNeded-paymentDto.Amount} zł.";
            case 0:
                payment = new Payment
                {
                    Amount = paymentDto.Amount,
                    ClientId = paymentDto.ClientId,
                    ContractId = contract.ContractId,
                    Date = DateTime.Now,
                };
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return "Zapłacono pełną kwote za kontrakt.";
            case < 0:
                var returnal = paymentDto.Amount - moneyNeded;
                payment = new Payment
                {
                    //Wpłacamy tylko wymaganą wartość
                    Amount = moneyNeded,
                    ContractId = contract.ContractId,
                    ClientId = paymentDto.ClientId,
                    Date = DateTime.Now
                };
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return $"Zapłacono z nadwyżką zwrócono {returnal} zł. Kontrakt już opłacony";
        }
    }


    private async Task<decimal> CalculatePriceAsync(int softwareVersionId, int additionalSupportYears)
    {
        var softwareVerison = await _context.SoftwareVersions.FindAsync(softwareVersionId);
        if (softwareVerison == null)
            throw new NotFoundException("Nie znaleziono wersji o takim numerze");
        var basePrice = softwareVerison.YearlyPrice;
        return basePrice + 1000 * additionalSupportYears;
    }

    private async Task<bool> IsReturningClient(int clientId)
    {
        var contracts = await _context.Contracts.Where(x => x.ClientId == clientId).ToListAsync();
        var subscriptions = await _context.Subscriptions.Where(x => x.ClientId == clientId).ToListAsync();
        return contracts.Count != 0 || subscriptions.Count != 0;
    }

    private async Task<decimal> FindBiggestDiscount(int softwareId)
    {
        var date = DateTime.Now;
        var discounts = await _context.Discounts
            .Include(d => d.Software)
            .Where(d => d.Software.Any(s => s.SoftwareId == softwareId))
            .ToListAsync(); // pobierz do pamięci PRZED filtrowaniem dat

        var best = discounts
            .Where(d =>
                (!d.IsRepetitive && d.StartDate <= date && d.EndDate >= date)
                ||
                (d.IsRepetitive &&
                 new DateTime(date.Year, d.StartDate.Month, d.StartDate.Day) <= date &&
                 new DateTime(date.Year, d.EndDate.Month, d.EndDate.Day) >= date)
            )
            .Select(d => (decimal?)d.DiscountPercent)
            .Max();

        return best ?? 0m;
    }
    
    private async Task<bool> HasActiveContract(int clientId, int sofwareId)
    {
        var contracts = await _context.Contracts.Where(x => x.IsActive && x.ClientId == clientId && x.SoftwareVersion.SoftwareId== sofwareId).ToListAsync();
        return contracts.Count != 0;
    }

    //Will return money that is to be paid
    //If over client overpaid, if under client underpaid,
    //if equal 0 contract is fully paid
    private async Task<decimal> CountTheMoneyDiff(int contractId)
    {
        var sumAsync = await _context.Payments.Where(x => x.ContractId == contractId).SumAsync(x => x.Amount);
        var contractAsync = await _context.Contracts.FindAsync(contractId);
        if (contractAsync == null)
            throw new NotFoundException("Nie znaleziono kontraktu");
        return contractAsync.TotalPrice - sumAsync;
    }
}