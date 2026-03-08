using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SwiftLux.WhatsApp.Api.Configurations;
using SwiftLux.WhatsApp.Api.Models.OpenAI;

namespace SwiftLux.WhatsApp.Api.Services
{
    public class OpenAIAgentService : IAgentService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAIOptions _options;
        private readonly ILogger<OpenAIAgentService> _logger;

        public OpenAIAgentService(
            HttpClient httpClient,
            IOptions<OpenAIOptions> options,
            ILogger<OpenAIAgentService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<AgentReplyResult> GetReplyAsync(string message)
        {
            var request = new
            {
                model = _options.Model,
                prompt = new
                {
                    id = _options.PromptId,
                    version = _options.PromptVersion
                },
                input = new object[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new
                            {
                                type = "input_text",
                                text = message + "\n\nReturn response strictly as JSON."
                            }
                        }
                    }
                },
                text = new
                {
                    format = new
                    {
                        type = "json_object"
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/responses");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            httpRequest.Content =
                new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(
                "OpenAI response. StatusCode: {StatusCode}, Body: {Body}",
                response.StatusCode,
                responseBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "OpenAI call failed. StatusCode: {StatusCode}, Body: {Body}",
                    response.StatusCode,
                    responseBody);

                return new AgentReplyResult
                {
                    IsSuccess = false,
                    RawJson = string.Empty,
                    ErrorMessage = "OpenAI API call failed."
                };
            }

            var rawJson = ExtractOutputText(responseBody);

            if (string.IsNullOrWhiteSpace(rawJson))
            {
                _logger.LogWarning("OpenAI response parsed, but no output text was found.");

                return new AgentReplyResult
                {
                    IsSuccess = false,
                    RawJson = string.Empty,
                    ErrorMessage = "No output text found in OpenAI response."
                };
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var parsed = JsonSerializer.Deserialize<BookingAssistantResult>(rawJson, options);

                if (parsed == null)
                {
                    return new AgentReplyResult
                    {
                        IsSuccess = false,
                        RawJson = rawJson,
                        ErrorMessage = "Failed to deserialize booking assistant result."
                    };
                }

                return new AgentReplyResult
                {
                    IsSuccess = true,
                    RawJson = rawJson,
                    Data = parsed
                };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize OpenAI JSON response. RawJson: {RawJson}", rawJson);

                return new AgentReplyResult
                {
                    IsSuccess = false,
                    RawJson = rawJson,
                    ErrorMessage = "Invalid JSON returned by OpenAI."
                };
            }
        }

        private static string? ExtractOutputText(string responseBody)
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            if (root.TryGetProperty("output_text", out var outputTextElement) &&
                outputTextElement.ValueKind == JsonValueKind.String)
            {
                var outputText = outputTextElement.GetString();
                if (!string.IsNullOrWhiteSpace(outputText))
                {
                    return outputText;
                }
            }

            if (root.TryGetProperty("output", out var outputElement) &&
                outputElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var outputItem in outputElement.EnumerateArray())
                {
                    if (outputItem.TryGetProperty("content", out var contentElement) &&
                        contentElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var contentItem in contentElement.EnumerateArray())
                        {
                            if (contentItem.TryGetProperty("text", out var textElement) &&
                                textElement.ValueKind == JsonValueKind.String)
                            {
                                var text = textElement.GetString();
                                if (!string.IsNullOrWhiteSpace(text))
                                {
                                    return text;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}