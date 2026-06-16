using Projekt.DTOs.RevenueDTOs;

namespace Projekt.Services;

public interface IRevenueService
{
    public Task<GetRevenueDTO> GetCurrentRevenueAsync(string? currency, string? softwareName);
}