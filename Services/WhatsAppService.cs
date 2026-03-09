using Microsoft.Extensions.Options;
using SwiftLux.WhatsApp.Api.Configurations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SwiftLux.WhatsApp.Api.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly WhatsAppOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(
            IOptions<WhatsAppOptions> options,
            HttpClient httpClient,
            ILogger<WhatsAppService> logger)
        {
            _options = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        
        public async Task SendTextMessageAsync(string to, string message)
        {
            var payload = new
            {
                messaging_product = "whatsapp",
                to = to,
                type = "text",
                text = new
                {
                    body = message
                }
            };

            var json = JsonSerializer.Serialize(payload);

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://graph.facebook.com/v23.0/{_options.PhoneNumberId}/messages");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.AccessToken);

            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(
                "WhatsApp send message response. StatusCode: {StatusCode}, Body: {Body}",
                response.StatusCode,
                responseBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to send WhatsApp message. To: {To}, StatusCode: {StatusCode}, Body: {Body}",
                    to,
                    response.StatusCode,
                    responseBody);

                throw new InvalidOperationException(
                    $"WhatsApp send failed. StatusCode: {response.StatusCode}, Body: {responseBody}");
            }
        }
    }
}