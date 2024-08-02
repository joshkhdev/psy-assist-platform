using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    private const string ContactNotFoundMessage = "Contact with Id [{0}] not found";
    private const string ContactCacheName = "Contact_{0}";
    private const string QuestionnaireCacheName = "Questionnaire_{0}";

    public ContactService(IRepository<Contact> contactRepository, IMapper applicationMapper, IMemoryCache memoryCache)
    {
        _contactRepository = contactRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IContact>?> GetContactsAsync(CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<ContactDto>>(contacts);
    }

    public async Task<IContact?> GetContactByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(ContactCacheName, id);
        var contact = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(12));

            var contactById = await _contactRepository.GetByIdAsync(id, cancellationToken);
            if (contactById is null)
                throw new NotFoundException(string.Format(ContactNotFoundMessage, id));

            return _applicationMapper.Map<ContactDto>(contactById);
        });

        return contact;
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
        
        _memoryCache.Remove(string.Format(ContactCacheName, id));
        _memoryCache.Remove(string.Format(QuestionnaireCacheName, id));
        _memoryCache.Remove(string.Format(QuestionnaireCacheName, "All"));

        return _applicationMapper.Map<ContactDto>(await _contactRepository.UpdateAsync(updatedContact, cancellationToken));
    }
}