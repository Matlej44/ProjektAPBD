using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Exceptions;
using Projekt.Services;

namespace Projekt.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;
        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }
        [HttpGet]
        public async Task<IActionResult> CurrentRevenue([FromQuery] string? currency, [FromQuery] string? softwareName)
        {
            try
            {
                var res = await _revenueService.GetCurrentRevenueAsync(currency, softwareName);
                return Ok(res);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("predict")]
        public async Task<IActionResult> PredictRevenue([FromQuery] string? currency, [FromQuery] string? softwareName)
        {
            try
            {
                var dto = await _revenueService.GetPredictedRevenueAsync(currency, softwareName);
                return Ok(dto);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
