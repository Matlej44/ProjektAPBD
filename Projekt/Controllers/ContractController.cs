using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Projekt.DTOs.ContractDTOs;
using Projekt.Exceptions;
using Projekt.Services;

namespace Projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }
        [HttpGet]
        public async Task<IActionResult> GetContracts()
        {
            var contracts = await _contractService.GetAllContractsAsync();
            return Ok(contracts);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody]AddContractDTO contractDto)
        {
            try
            {
                var contract = await _contractService.CreateContract(contractDto);
                return CreatedAtAction(nameof(GetContractById), new {id = contract.ContractId}, contract);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500 ,e.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            try
            {
                var contract = await _contractService.GetContractByIdAsync(id);
                return Ok(contract);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
    
}
