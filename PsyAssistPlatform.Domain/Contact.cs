namespace PsyAssistPlatform.Domain;

/// <summary>
/// Контакт
/// </summary>
public sealed class Contact
{
    public int Id { get; set; }

    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}

