using Microsoft.EntityFrameworkCore;
using SwiftLux.WhatsApp.Api.Data;
using SwiftLux.WhatsApp.Api.Models;

namespace SwiftLux.WhatsApp.Api.Services
{
    public class ConversationService : IConversationService
    {
        private readonly AppDbContext _db;

        public ConversationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Conversation> GetOrCreateConversationAsync(string phone)
        {
            var conversation =
                await _db.Conversations
                    .FirstOrDefaultAsync(x => x.CustomerPhone == phone);

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    CustomerPhone = phone,
                    CreatedAt = DateTime.UtcNow,
                    LastMessageAt = DateTime.UtcNow
                };

                _db.Conversations.Add(conversation);
                await _db.SaveChangesAsync();
            }

            return conversation;
        }

        public async Task SaveInboundMessageAsync(Guid conversationId, string message)
        {
            var msg = new MessageLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                Direction = "inbound",
                MessageText = message,
                CreatedAt = DateTime.UtcNow
            };

            _db.Messages.Add(msg);

            await _db.SaveChangesAsync();
        }

        public async Task SaveOutboundMessageAsync(Guid conversationId, string message)
        {
            var msg = new MessageLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                Direction = "outbound",
                MessageText = message,
                CreatedAt = DateTime.UtcNow
            };

            _db.Messages.Add(msg);

            await _db.SaveChangesAsync();
        }
    }
}