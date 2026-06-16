using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt.DTOs.ClientsDTOs;
using Projekt.Exceptions;
using Projekt.Services;

namespace Projekt.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("person")]
        public async Task<IActionResult> CreatePerson([FromBody] AddPersonDTO addPersonDto)
        {
            try
            {
                await _clientService.AddPersonAsync(addPersonDto);
                return Created();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("person/{id:int}")]
        public async Task<IActionResult> ModifyPerson([FromRoute] int id, [FromBody] ModifyPersonDTO modifyPersonDto)
        {
            try
            {
                await _clientService.ModifyPersonAsync(id, modifyPersonDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpDelete]
        [Route("person/{id:int}")]
        public async Task<IActionResult> DeletePerson([FromRoute] int id)
        {
            try
            {
                await _clientService.DeletePersonAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("person")]
        public async Task<IActionResult> GetPersons()
        {
            var people = await _clientService.GetPersonsAsync();
            return Ok(people);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("company")]
        public async Task<IActionResult> CreateCompany([FromBody] AddCompanyDTO addCompanyDto)
        {

            try
            {
                await _clientService.AddCompanyAsync(addCompanyDto);
                return Created();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("company/{id:int}")]
        public async Task<IActionResult> ModifyCompany([FromRoute] int id, [FromBody] ModifyCompanyDTO modifyCompanyDto)
        {
            try
            {
                await _clientService.ModifyCompanyAsync(id, modifyCompanyDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpGet]
        [Route("company")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _clientService.GetCompaniesAsync();
            return Ok(companies);
        }
    }
}
