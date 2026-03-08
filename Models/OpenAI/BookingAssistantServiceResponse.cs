namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class BookingAssistantServiceResponse
    {
        public bool IsSuccess { get; set; }

        public string RawResponse { get; set; } = string.Empty;

        public BookingAssistantResult? ParsedResult { get; set; }

        public string? ErrorMessage { get; set; }
    }
}