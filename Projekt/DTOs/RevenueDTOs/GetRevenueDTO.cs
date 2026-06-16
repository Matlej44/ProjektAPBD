namespace Projekt.DTOs.RevenueDTOs;

public class GetRevenueDTO
{
    public decimal OverallRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }
    public Dictionary<string, decimal> MonthlyRevenue { get; set; }
    public decimal YearlySubsriptionRevenue { get; set; }
    public decimal YearlyContractRevenue { get; set; }
}