namespace PsyAssistPlatform.WebApi.Models.Feedback;

public record FeedbackResponse
{
    public int Id { get; set; }

    public required string Telegram { get; set; }

    public required DateTime? FeedbackDate { get; set; }

    public required string FeedbackText { get; set; }
}
