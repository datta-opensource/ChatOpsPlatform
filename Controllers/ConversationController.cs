using Microsoft.AspNetCore.Mvc;
using SwiftLux.WhatsApp.Api.Services;

namespace SwiftLux.WhatsApp.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly InMemoryConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = (InMemoryConversationService)conversationService;
        }

        [HttpGet]
        public IActionResult GetConversations()
        {
            return Ok(_conversationService.GetAllConversations());
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            return Ok(_conversationService.GetAllMessages());
        }
    }
}