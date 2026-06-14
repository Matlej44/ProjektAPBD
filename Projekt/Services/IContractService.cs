using Projekt.DTOs.ContractDTOs;

namespace Projekt.Services;

public interface IContractService
{
    public Task<List<GetContractsDTO>> GetAllContractsAsync();
    public Task<GetContractsDTO> GetContractByIdAsync(int id);
    public Task<GetContractsDTO> CreateContract(AddContractDTO contractDto);
    public Task UpdateAllContractsAsync();
    public Task DeleteContractAsync(int id);
}