namespace SwiftLux.WhatsApp.Api.Configurations
{
    public class WhatsAppOptions
    {
        public const string SectionName = "WhatsApp";

        public string AccessToken { get; set; } = string.Empty;

        public string PhoneNumberId { get; set; } = string.Empty;

        public string VerifyToken { get; set; } = string.Empty;
    }
}