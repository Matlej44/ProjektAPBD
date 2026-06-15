namespace Projekt.Services;

public interface IRevenueService
{
    public Task<string> GetCurrentRevenueAsync(string? currency);
}