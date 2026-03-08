using SwiftLux.WhatsApp.Api.Models;
using SwiftLux.WhatsApp.Api.Models.OpenAI;

namespace SwiftLux.WhatsApp.Api.Services
{
    public class WebhookEventProcessor
    {
        private readonly ILogger<WebhookEventProcessor> _logger;
        private readonly IWhatsAppService _whatsAppService;
        private readonly ICommandHandler _commandHandler;
        private readonly IConversationService _conversationService;
        private readonly IAgentService _agentService;

        public WebhookEventProcessor(
            ILogger<WebhookEventProcessor> logger,
            IWhatsAppService whatsAppService,
            ICommandHandler commandHandler,
            IConversationService conversationService,
            IAgentService agentService)
        {
            _logger = logger;
            _whatsAppService = whatsAppService;
            _commandHandler = commandHandler;
            _conversationService = conversationService;
            _agentService = agentService;
        }

        public async Task ProcessAsync(WhatsAppWebhookRequest payload, string rawBody)
        {
            _logger.LogInformation("========== WHATSAPP WEBHOOK RECEIVED ==========");
            _logger.LogInformation("Raw Payload: {RawPayload}", rawBody);

            if (payload?.Entry == null || !payload.Entry.Any())
            {
                _logger.LogWarning("Webhook payload has no entries.");
                return;
            }

            foreach (var entry in payload.Entry)
            {
                if (entry.Changes == null || !entry.Changes.Any())
                {
                    continue;
                }

                foreach (var change in entry.Changes)
                {
                    await ProcessMessagesAsync(change.Value);
                    ProcessStatuses(change.Value);
                }
            }
        }

        private async Task ProcessMessagesAsync(Value? value)
        {
            var messages = value?.Messages;

            if (messages == null || !messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                var from = message.From;
                var text = message.Text?.Body;
                var type = message.Type;

                _logger.LogInformation(
                    "Incoming WhatsApp message. From: {From}, Type: {Type}, Text: {Text}",
                    from,
                    type,
                    text);

                if (string.IsNullOrWhiteSpace(from) ||
                    !string.Equals(type, "text", StringComparison.OrdinalIgnoreCase) ||
                    string.IsNullOrWhiteSpace(text))
                {
                    _logger.LogInformation(
                        "Skipping message. From: {From}, Type: {Type}",
                        from,
                        type);
                    continue;
                }

                var conversation = await _conversationService.GetOrCreateConversationAsync(from);
                await _conversationService.SaveInboundMessageAsync(conversation.Id, text);

                string reply;
                var commandReply = _commandHandler.GetReply(text);

                if (commandReply.StartsWith("Received your message"))
                {
                    try
                    {
                        var aiResult = await _agentService.GetReplyAsync(text);

                        if (aiResult.IsSuccess &&
                            aiResult.Data != null &&
                            !string.IsNullOrWhiteSpace(aiResult.Data.CustomerReply))
                        {
                            reply = aiResult.Data.CustomerReply;

                            _logger.LogInformation(
                                "AI reply generated successfully. Intent: {Intent}, Status: {Status}, RawJson: {RawJson}",
                                aiResult.Data.Intent,
                                aiResult.Data.Status,
                                aiResult.RawJson);
                        }
                        else
                        {
                            _logger.LogWarning(
                                "AI reply generation failed or returned empty customer reply. Error: {ErrorMessage}, RawJson: {RawJson}",
                                aiResult.ErrorMessage,
                                aiResult.RawJson);

                            reply = "Thank you for your message. Our team will assist you shortly.";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "AI agent failed. Falling back to default reply.");
                        reply = "Thank you for your message. Our team will assist you shortly.";
                    }
                }
                else
                {
                    reply = commandReply;
                }

                try
                {
                    await _whatsAppService.SendTextMessageAsync(from, reply);
                    await _conversationService.SaveOutboundMessageAsync(conversation.Id, reply);

                    _logger.LogInformation(
                        "Reply sent successfully. To: {To}, Reply: {Reply}",
                        from,
                        reply);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Failed to send WhatsApp reply. To: {To}, Reply: {Reply}",
                        from,
                        reply);
                }
            }
        }

        private void ProcessStatuses(Value? value)
        {
            var statuses = value?.Statuses;

            if (statuses == null || !statuses.Any())
            {
                return;
            }

            foreach (var status in statuses)
            {
                _logger.LogInformation(
                    "WhatsApp status update. MessageId: {MessageId}, Recipient: {Recipient}, Status: {Status}, Timestamp: {Timestamp}",
                    status.Id,
                    status.RecipientId,
                    status.Status,
                    status.Timestamp);
            }
        }
    }
}