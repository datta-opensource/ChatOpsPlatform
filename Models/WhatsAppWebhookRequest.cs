using System.Text.Json.Serialization;

namespace SwiftLux.WhatsApp.Api.Models
{
    public class WhatsAppWebhookRequest
    {
        [JsonPropertyName("entry")]
        public List<Entry>? Entry { get; set; }
    }

    public class Entry
    {
        [JsonPropertyName("changes")]
        public List<Change>? Changes { get; set; }
    }

    public class Change
    {
        [JsonPropertyName("value")]
        public Value? Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("messages")]
        public List<Message>? Messages { get; set; }

        [JsonPropertyName("statuses")]
        public List<StatusUpdate>? Statuses { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("from")]
        public string? From { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public MessageText? Text { get; set; }
    }

    public class MessageText
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }

    public class StatusUpdate
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("recipient_id")]
        public string? RecipientId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }
    }
}