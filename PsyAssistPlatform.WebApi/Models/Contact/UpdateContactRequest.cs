using PsyAssistPlatform.Application.Interfaces.Dto.Contact;

namespace PsyAssistPlatform.WebApi.Models.Contact;

public record UpdateContactRequest : IUpdateContact
{
    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}
