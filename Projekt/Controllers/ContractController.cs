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
                return StatusCode(500);
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
        
        //Function actvates every day an updates if active or deactivates if not active
        [HttpPut]
        public async Task<IActionResult> UpdateAllActiveContracts()
        {
            await _contractService.UpdateAllContractsAsync();
            return Ok();
        }
        //Function deletes contract if not active
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAllInactiveContracts([FromHeader] int id)
        {
            try
            {
                await _contractService.DeleteContractAsync(id);
                return NoContent();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/payments")]
        public async Task<IActionResult> CreatePayment([FromHeader] int id, [FromBody] AddPaymentDTO addPaymentDtoDto)
        {
            try
            {
                var payment = await _contractService.CreatePaymentAsync(id, addPaymentDtoDto);
                return Ok(payment);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            
        }
    }
    
}
