using System.Text.Json.Serialization;

namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class BookingAssistantResult
    {
        [JsonPropertyName("intent")]
        public string? Intent { get; set; }

        [JsonPropertyName("customer_reply")]
        public string? CustomerReply { get; set; }

        [JsonPropertyName("trip_details")]
        public TripDetails? TripDetails { get; set; }

        [JsonPropertyName("fare_details")]
        public FareDetails? FareDetails { get; set; }

        [JsonPropertyName("missing_fields")]
        public List<string> MissingFields { get; set; } = new();

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}