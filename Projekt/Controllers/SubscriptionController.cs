using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.DTOs.SubscriptionDTOs;
using Projekt.Exceptions;
using Projekt.Services;

namespace Projekt.Controllers
{
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
        public async Task<IActionResult> PurchaseSubscription([FromRoute] int offerId, [FromBody] CreateSubscriptionDTO dto)
        {
            try
            {
                var subscription = _subscrptionService.CreateSubscrption(offerId, dto);
                return CreatedAtAction(nameof(GetSubscription), new {subscriptionId = subscription.Id},subscription);
            }
            catch (Exception)
            {
                return StatusCode(500);
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
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
