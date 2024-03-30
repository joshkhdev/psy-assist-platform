namespace PsyAssistPlatform.Domain;

/// <summary>
/// Контакт
/// </summary>
public class Contact : BaseEntity
{
    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}

