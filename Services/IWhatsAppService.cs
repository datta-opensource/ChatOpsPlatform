namespace SwiftLux.WhatsApp.Api.Services
{
    public interface IWhatsAppService
    {
        Task SendTextMessageAsync(string to, string message);
    }
}