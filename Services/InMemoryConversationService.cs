using SwiftLux.WhatsApp.Api.Models;

namespace SwiftLux.WhatsApp.Api.Services
{
    public class InMemoryConversationService : IConversationService
    {
        private static readonly List<Conversation> _conversations = new();
        private static readonly List<MessageLog> _messages = new();

        public Task<Conversation> GetOrCreateConversationAsync(string customerPhone)
        {
            var conversation = _conversations
                .FirstOrDefault(x => x.CustomerPhone == customerPhone);

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    CustomerPhone = customerPhone,
                    CreatedAt = DateTime.UtcNow,
                    LastMessageAt = DateTime.UtcNow
                };

                _conversations.Add(conversation);
            }

            return Task.FromResult(conversation);
        }

        public Task SaveInboundMessageAsync(Guid conversationId, string message)
        {
            _messages.Add(new MessageLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                Direction = "inbound",
                MessageText = message,
                CreatedAt = DateTime.UtcNow
            });

            return Task.CompletedTask;
        }
        public List<Conversation> GetAllConversations()
        {
            return _conversations;
        }

        public List<MessageLog> GetAllMessages()
        {
            return _messages;
        }

        public Task SaveOutboundMessageAsync(Guid conversationId, string message)
        {
            _messages.Add(new MessageLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                Direction = "outbound",
                MessageText = message,
                CreatedAt = DateTime.UtcNow
            });

            return Task.CompletedTask;
        }
    }
}