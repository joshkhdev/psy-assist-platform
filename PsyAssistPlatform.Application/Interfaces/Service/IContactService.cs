using PsyAssistPlatform.Application.Interfaces.Dto.Contact;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IContactService
{
    Task<IEnumerable<IContact>> GetContactsAsync(CancellationToken cancellationToken);
    
    Task<IContact?> GetContactByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IContact> UpdateContactAsync(int id, IUpdateContact contactData, CancellationToken cancellationToken);
}