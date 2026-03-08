using SwiftLux.WhatsApp.Api.Models.OpenAI;

namespace SwiftLux.WhatsApp.Api.Services
{
    public interface IAgentService
    {
        Task<AgentReplyResult> GetReplyAsync(string message);
    }
}