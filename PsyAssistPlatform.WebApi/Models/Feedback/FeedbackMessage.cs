namespace PsyAssistPlatform.Messages;

public class FeedbackMessage
{
    public int Id { get; set; }

    public required string Telegram { get; set; }

    public required DateTime? FeedbackDate { get; set; }

    public required string FeedbackText { get; set; }
}
