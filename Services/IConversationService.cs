using SwiftLux.WhatsApp.Api.Models;

namespace SwiftLux.WhatsApp.Api.Services
{
    public interface IConversationService
    {
        Task<Conversation> GetOrCreateConversationAsync(string customerPhone);

        Task SaveInboundMessageAsync(Guid conversationId, string message);

        Task SaveOutboundMessageAsync(Guid conversationId, string message);
    }
}