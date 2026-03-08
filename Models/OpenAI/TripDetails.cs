using System.Text.Json.Serialization;

namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class TripDetails
    {
        [JsonPropertyName("pickup_city")]
        public string? PickupCity { get; set; }

        [JsonPropertyName("pickup_locality")]
        public string? PickupLocality { get; set; }

        [JsonPropertyName("drop_city")]
        public string? DropCity { get; set; }

        [JsonPropertyName("drop_locality")]
        public string? DropLocality { get; set; }

        [JsonPropertyName("trip_type")]
        public string? TripType { get; set; }

        [JsonPropertyName("rental_type")]
        public string? RentalType { get; set; }

        [JsonPropertyName("package_hours")]
        public int? PackageHours { get; set; }

        [JsonPropertyName("vehicle_type")]
        public string? VehicleType { get; set; }

        [JsonPropertyName("estimated_distance_km")]
        public decimal? EstimatedDistanceKm { get; set; }

        [JsonPropertyName("days")]
        public int? Days { get; set; }

        [JsonPropertyName("billable_km")]
        public decimal? BillableKm { get; set; }

        [JsonPropertyName("is_fixed_package")]
        public bool? IsFixedPackage { get; set; }
    }
}