namespace Projekt.DTOs.RevenueDTOs;

public class GetPredictedRevenue
{
    public decimal CurrentYearlyRevenue { get; set; }
    public decimal PredictedYearlyRevenue { get; set; }
    public decimal PredictedRevenuePercentage { get; set; }
    
    public decimal CurrentYearlyRevenueContracts { get; set; }
    public decimal PredictedYearlyRevenueContracts { get; set; }
    
    public decimal CurrentYearlyRevenueSubscriptions { get; set; }
    public decimal PredictedYearlyRevenueSubscriptions { get; set; }

}