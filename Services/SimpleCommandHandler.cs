namespace SwiftLux.WhatsApp.Api.Services
{
    public class SimpleCommandHandler : ICommandHandler
    {
        public string GetReply(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                return "Welcome to SwiftLux Bot 🚖";
            }

            var normalizedText = messageText.Trim().ToLowerInvariant();

            return normalizedText switch
            {
                "hi" => "Hello from SwiftLux Bot 🚖",
                "hello" => "Hello from SwiftLux Bot 🚖",
                "help" => "Available commands: hi, help, book cab",
                "book cab" => "Please share your pickup location, drop location, date and time.",
                _ => $"Received your message: {messageText}"
            };
        }
    }
}