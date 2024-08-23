using PsyAssistPlatform.WebApi.Models.Feedback;

namespace PsyAssistPlatform.Messages;

public record FeedbacksMessage
{
    public required IEnumerable<FeedbackResponse> Items { get; set; }
}
