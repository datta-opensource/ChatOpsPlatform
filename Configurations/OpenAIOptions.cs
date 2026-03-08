namespace SwiftLux.WhatsApp.Api.Configurations
{
    public class OpenAIOptions
    {
        public const string SectionName = "OpenAI";
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public string PromptId { get; set; }
        public string PromptVersion { get; set; }
    }
}