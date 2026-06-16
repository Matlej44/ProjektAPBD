using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.DTOs.ContractDTOs;
using Projekt.DTOs.SubscriptionDTOs;
using Projekt.Exceptions;
using Projekt.Services;

namespace Projekt.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscrptionService _subscrptionService;

        public SubscriptionController(ISubscrptionService subscrptionService)
        {
            _subscrptionService = subscrptionService;
        }

        [HttpPost]
        [Route("{offerId:int}")]
        public async Task<IActionResult> PurchaseSubscription([FromRoute] int offerId,
            [FromBody] CreateSubscriptionDTO dto)
        {
            try
            {
                var subscription = await _subscrptionService.CreateSubscrption(offerId, dto);
                return CreatedAtAction(nameof(GetSubscription), new { subscriptionId = subscription.SubsriptionId },
                    subscription);
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
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("{subscriptionId:int}/payments")]
        public async Task<IActionResult> CreatePayment([FromRoute] int subscriptionId,
            [FromBody] PaymentDTO paymentDto)
        {
            try
            {
                var messege = await _subscrptionService.CreatePayment(subscriptionId, paymentDto);
                return Ok(messege);
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
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("offers/{softwareId:int}")]
        public async Task<IActionResult> GetSubscriptionOffers([FromRoute] int softwareId)
        {
            try
            {
                var offers = await _subscrptionService.GetSubscriptionOffer(softwareId);
                return Ok(offers);
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
        [Route("{subscriptionId:int}")]
        public async Task<IActionResult> GetSubscription([FromRoute] int subscriptionId)
        {
            try
            {
                var subscription = await _subscrptionService.GetSubscription(subscriptionId);
                return Ok(subscription);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}