using System.ComponentModel.DataAnnotations;

namespace SwiftLux.WhatsApp.Api.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastMessageAt { get; set; }

        public List<MessageLog> Messages { get; set; }
    }
}