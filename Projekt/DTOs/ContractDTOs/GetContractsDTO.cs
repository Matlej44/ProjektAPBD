namespace Projekt.DTOs.ContractDTOs;

public class GetContractsDTO
{
    public int ContractId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ClientEmail { get; set; }
    public int AdditionalSupportYears { get; set;}
    public string SoftwareVersion { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsActive { get; set; }
}