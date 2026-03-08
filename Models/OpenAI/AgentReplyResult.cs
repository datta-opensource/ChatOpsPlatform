namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class AgentReplyResult
    {
        public bool IsSuccess { get; set; }

        public string RawJson { get; set; } = string.Empty;

        public BookingAssistantResult? Data { get; set; }

        public string? ErrorMessage { get; set; }
    }
}