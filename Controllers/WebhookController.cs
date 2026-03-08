using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SwiftLux.WhatsApp.Api.Configurations;
using SwiftLux.WhatsApp.Api.Models;
using SwiftLux.WhatsApp.Api.Services;

namespace SwiftLux.WhatsApp.Api.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly WhatsAppOptions _whatsAppOptions;
        private readonly ILogger<WebhookController> _logger;
        private readonly WebhookEventProcessor _webhookEventProcessor;

        public WebhookController(
            IOptions<WhatsAppOptions> whatsAppOptions,
            ILogger<WebhookController> logger,
            WebhookEventProcessor webhookEventProcessor)
        {
            _whatsAppOptions = whatsAppOptions.Value;
            _logger = logger;
            _webhookEventProcessor = webhookEventProcessor;
        }

        [HttpGet]
        public IActionResult VerifyWebhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.verify_token")] string verifyToken,
            [FromQuery(Name = "hub.challenge")] string challenge)
        {
            _logger.LogInformation(
                "Webhook verification request received. Mode: {Mode}, VerifyToken: {VerifyToken}",
                mode,
                verifyToken);

            if (mode == "subscribe" && verifyToken == _whatsAppOptions.VerifyToken)
            {
                _logger.LogInformation("Webhook verification successful.");
                return Content(challenge, "text/plain");
            }

            _logger.LogWarning("Webhook verification failed.");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WhatsAppWebhookRequest payload)
        {
            try
            {
                Request.EnableBuffering();

                Request.Body.Position = 0;
                using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
                var rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                await _webhookEventProcessor.ProcessAsync(payload, rawBody);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing WhatsApp webhook.");
                return Ok();
            }
        }
    }
}