using PsyAssistPlatform.Application.Interfaces.Dto.Contact;

namespace PsyAssistPlatform.Application.Dto.Contact;

public record ContactDto : IContact
{
    public int Id { get; set; }
    
    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}