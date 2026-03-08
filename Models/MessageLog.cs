using System.ComponentModel.DataAnnotations;

namespace SwiftLux.WhatsApp.Api.Models
{
    public class MessageLog
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        public string Direction { get; set; }

        public string MessageText { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}