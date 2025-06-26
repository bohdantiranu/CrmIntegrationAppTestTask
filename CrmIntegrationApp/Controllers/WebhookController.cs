using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Exceptions;
using CrmIntegrationApp.Helpers;
using CrmIntegrationApp.Models;
using CrmIntegrationApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CrmIntegrationApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookProcessorService _webhookProcessorService;
        private readonly ILogger<WebhookController> _logger;

        private readonly WebhookJwtSettings _jwtSettings;
        public WebhookController(IWebhookProcessorService webhookProcessorService, 
            ILogger<WebhookController> logger,
            IOptions<WebhookJwtSettings> jwtOptions)
        {
            _webhookProcessorService = webhookProcessorService;
            _logger = logger;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("newTicket")]
        public async Task<IActionResult> ProcessNewTicketAsync([FromBody] CrmTicket crmTicket)
        {
            if (crmTicket == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("Received new ticket {TicketId}", crmTicket.Id);
            try
            {
                await _webhookProcessorService.ProcessWebhookTicketAsync(crmTicket);
            }
            catch (TicketProcessingFailedException ex) 
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { eror = ex.Message });
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("getFakeToken")]
        public async Task<IActionResult> GetFakeToken()
        {
            return Ok(FakeJwtGenerator.GenerateToken(_jwtSettings.Issuer, _jwtSettings.Audience, _jwtSettings.SecretKey));
        }
    }
}