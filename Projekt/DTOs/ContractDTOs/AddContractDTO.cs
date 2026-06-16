namespace Projekt.DTOs.ContractDTOs;

public class AddContractDTO
{
    public int ClientId { get; set; }
    public int AdditionalSupportYears { get; set; }
    public int SoftwareId { get; set; }
    public string SoftwareVersion { get; set; }
}