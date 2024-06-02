using AutoMapper;
using PsyAssistPlatform.Application.Dto.Contact;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.Contact;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class ContactService : IContactService
{
    private readonly IRepository<Contact> _contactRepository;
    private readonly IMapper _applicationMapper;
    private const string ContactNotFoundMessage = "Contact with Id [{0}] not found";

    public ContactService(IRepository<Contact> contactRepository, IMapper applicationMapper)
    {
        _contactRepository = contactRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IContact>> GetContactsAsync(CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<ContactDto>>(contacts);
    }

    public async Task<IContact?> GetContactByIdAsync(int id, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);
        if (contact is null)
            throw new NotFoundException(string.Format(ContactNotFoundMessage, id));
        
        return _applicationMapper.Map<ContactDto>(contact);
    }

    public async Task<IContact> UpdateContactAsync(int id, IUpdateContact contactData, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);
        
        if (contact is null)
            throw new NotFoundException(string.Format(ContactNotFoundMessage, id));

        if (string.IsNullOrWhiteSpace(contactData.Email)
            && string.IsNullOrWhiteSpace(contactData.Phone)
            && string.IsNullOrWhiteSpace(contactData.Telegram))
        {
            throw new IncorrectDataException("All contact details (email, phone, telegram) cannot be empty");
        }

        if (!string.IsNullOrWhiteSpace(contactData.Email))
        {
            if (!Validator.EmailValidator(contactData.Email))
                throw new IncorrectDataException("Incorrect email address format");
        }
        
        if (!string.IsNullOrWhiteSpace(contactData.Phone))
        {
            if (!Validator.PhoneNumberValidator(contactData.Phone))
                throw new IncorrectDataException("Incorrect phone number format");
        }

        var updatedContact = _applicationMapper.Map<Contact>(contactData);
        updatedContact.Id = id;

        return _applicationMapper.Map<ContactDto>(await _contactRepository.UpdateAsync(updatedContact, cancellationToken));
    }
}