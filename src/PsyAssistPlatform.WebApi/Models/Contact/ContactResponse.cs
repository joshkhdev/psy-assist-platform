namespace PsyAssistPlatform.WebApi.Models.Contact;

public record ContactResponse
{
    public int Id { get; set; }

    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}
