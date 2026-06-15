using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.DTOs.ClientsDTOs;
using Projekt.Entity;
using Projekt.Exceptions;

namespace Projekt.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;
    public ClientService(AppDbContext context)
    {
        _context = context;
        
    }
    public async Task AddPersonAsync(AddPersonDTO personDto)
    {
        try
        {
            var clientPerson = new ClientPerson
            {
                Email = personDto.Email,
                Name = personDto.Name,
                Surname = personDto.Surname,
                Pesel = personDto.Pesel,
                PhoneNumber = personDto.PhoneNumber
            };
            var person = await _context.ClientPersons.AddAsync(clientPerson);
            if (person == null)
                throw new BadRequestException("Nie udało się wstawić osoby");
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new BadRequestException("Osoba o takim peselu już istnieje");
        }
    }

    public async Task AddCompanyAsync(AddCompanyDTO companyDto)
    {
        try
        {
            var comapny = new ClientCompany
            {
                Email = companyDto.Email,
                PhoneNumber = companyDto.PhoneNumber,
                CompanyName = companyDto.CompanyName,
                Address = companyDto.Address,
                KRS = companyDto.KRS
            };
            var company = await _context.ClientCompanies.AddAsync(comapny);
            if (company == null)
                throw new BadRequestException("Nie udało się wstawić firmy");
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new BadRequestException("Firma o takim KRS już istnieje");
        }
    }

    public async Task ModifyPersonAsync(int id, ModifyPersonDTO personDto)
    {
        var person = await _context.ClientPersons.FindAsync(id);
        if (person==null)
            throw new NotFoundException("Nie znaleziono osoby o tym id");
        person.Name = personDto.Name;
        person.Surname = personDto.Surname;
        person.PhoneNumber = personDto.PhoneNumber;
        person.Email = personDto.Email;
        _context.ClientPersons.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task ModifyCompanyAsync(int id, ModifyCompanyDTO companyDto)
    {
        var company = await _context.ClientCompanies.FindAsync(id);
        if (company==null)
            throw new NotFoundException("Nie znaleziono firmy o tym id");
        company.Address =companyDto.Address;
        company.PhoneNumber = companyDto.PhoneNumber;
        company.CompanyName = companyDto.CompanyName;
        company.Email = companyDto.Email;
        _context.ClientCompanies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePersonAsync(int id)
    {
        var person = await _context.ClientPersons.FindAsync(id);
        if (person==null)
            throw new NotFoundException("Nie znaleziono osoby o tym id");
        _context.ClientPersons.Remove(person);
        await _context.SaveChangesAsync();
    }

    public async Task<List<GetCompanyDTO>> GetCompaniesAsync()
    {
        var allAsync = await _context.ClientCompanies.Select(x => new GetCompanyDTO
        {
            Address = x.Address,
            CompanyName = x.CompanyName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            KRS = x.KRS,
            Id = x.ClientId
        }).ToListAsync();
        return allAsync;
    }

    public async Task<List<GetPersonDTO>> GetPersonsAsync()
    {
        var people = await _context.ClientPersons.Select(x => new GetPersonDTO
        {
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Name = x.Name,
            Surname = x.Surname,
            Pesel = x.Pesel,
            Id = x.ClientId
        }).ToListAsync();
        return people;
    }
}