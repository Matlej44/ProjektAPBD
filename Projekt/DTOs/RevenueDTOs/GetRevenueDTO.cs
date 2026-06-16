namespace Projekt.DTOs.RevenueDTOs;

public class GetRevenueDTO
{
    public decimal OverallRevenue { get; set; }
    public decimal OverallRevenueContracts { get; set; }
    public decimal OverallRevenueSubscriptions { get; set; }
    public decimal YearlyRevenue { get; set; }
    public decimal YearlySubsriptionRevenue { get; set; }
    public decimal YearlyContractRevenue { get; set; }
    public Dictionary<string, decimal> MonthlyRevenueContracts { get; set; }
    public Dictionary<string, decimal> MonthlyRevenueSubscriptions { get; set; }
    public Dictionary<string, decimal> MonthlyRevenueSoftware { get; set; }
}