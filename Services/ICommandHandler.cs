namespace SwiftLux.WhatsApp.Api.Services
{
    public interface ICommandHandler
    {
        string GetReply(string messageText);
    }
}