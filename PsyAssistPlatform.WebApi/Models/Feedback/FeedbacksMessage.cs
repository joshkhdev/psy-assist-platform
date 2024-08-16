using PsyAssistPlatform.WebApi.Models.Feedback;

namespace PsyAssistPlatform.Messages;

public class FeedbacksMessage
{
    public required IEnumerable<FeedbackResponse> Items { get; set; }
}
