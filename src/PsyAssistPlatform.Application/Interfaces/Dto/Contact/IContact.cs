namespace PsyAssistPlatform.Application.Interfaces.Dto.Contact;

public interface IContact
{
    int Id { get; set; }
    
    string? Telegram { get; set; }

    string? Email { get; set; }

    string? Phone { get; set; }
}