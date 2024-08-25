namespace PsyAssistPlatform.Application.Interfaces.Dto.Contact;

public interface IUpdateContact
{
    string? Telegram { get; set; }

    string? Email { get; set; }

    string? Phone { get; set; }
}