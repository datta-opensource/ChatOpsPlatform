namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class BookingAssistantRequest
    {
        public string UserMessage { get; set; } = string.Empty;

        public string? CustomerPhoneNumber { get; set; }

        public string? CustomerName { get; set; }

        public List<string> ConversationHistory { get; set; } = new();
    }
}