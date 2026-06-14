using Projekt.DTOs.ClientsDTOs;
using Projekt.Entity;

namespace Projekt.Services;

public interface IClientService
{
    public Task AddPersonAsync(AddPersonDTO personDto);
    public Task AddCompanyAsync(AddCompanyDTO companyDto);
    public Task ModifyPersonAsync(int id,ModifyPersonDTO personDto);
    public Task ModifyCompanyAsync(int id,ModifyCompanyDTO companyDto);
    public Task DeletePersonAsync(int id);
    public Task<List<GetCompanyDTO>> GetCompaniesAsync();
    public Task<List<GetPersonDTO>> GetPersonsAsync();
}