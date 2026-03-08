using System.Text.Json.Serialization;

namespace SwiftLux.WhatsApp.Api.Models.OpenAI
{
    public class FareDetails
    {
        [JsonPropertyName("sedan_fare")]
        public decimal? SedanFare { get; set; }

        [JsonPropertyName("suv_fare")]
        public decimal? SuvFare { get; set; }

        [JsonPropertyName("crysta_fare")]
        public decimal? CrystaFare { get; set; }

        [JsonPropertyName("extra_km_rate")]
        public decimal? ExtraKmRate { get; set; }

        [JsonPropertyName("driver_allowance_per_day")]
        public decimal? DriverAllowancePerDay { get; set; }

        [JsonPropertyName("toll_parking_extra")]
        public bool? TollParkingExtra { get; set; }
    }
}