using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Dto.Questionnaire;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IRepository<Questionnaire> _questionnaireRepository;
    private readonly IRepository<Contact> _contactRepository;
    private readonly IMapper _applicationMapper;
    private readonly IMemoryCache _memoryCache;
    private const string QuestionnaireCacheName = "Questionnaire_{0}";

    public QuestionnaireService(
        IRepository<Questionnaire> questionnaireRepository, 
        IRepository<Contact> contactRepository, 
        IMapper applicationMapper,
        IMemoryCache memoryCache)
    {
        _questionnaireRepository = questionnaireRepository;
        _contactRepository = contactRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IQuestionnaire>?> GetQuestionnairesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(QuestionnaireCacheName, "All");
        var questionnaires = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
            
            var allQuestionnaires = await _questionnaireRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<QuestionnaireDto>>(allQuestionnaires);
        });

        return questionnaires;
    }

    public async Task<IQuestionnaire?> GetQuestionnaireByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(QuestionnaireCacheName, id);
        var questionnaire = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(24));

            var questionnaireById = await _questionnaireRepository.GetByIdAsync(id, cancellationToken);
            if (questionnaireById is null)
                throw new NotFoundException($"Questionnaire with Id [{id}] not found");

            return _applicationMapper.Map<QuestionnaireDto>(questionnaireById);
        });

        return questionnaire;
    }

    public async Task<IQuestionnaire> CreateQuestionnaireAsync(ICreateQuestionnaire questionnaireData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(questionnaireData.Email)
            && string.IsNullOrWhiteSpace(questionnaireData.Phone)
            && string.IsNullOrWhiteSpace(questionnaireData.Telegram))
        {
            throw new IncorrectDataException("All contact details (email, phone, telegram) cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(questionnaireData.Name)
            || string.IsNullOrWhiteSpace(questionnaireData.Pronouns)
            || string.IsNullOrWhiteSpace(questionnaireData.TimeZone)
            || string.IsNullOrWhiteSpace(questionnaireData.NeuroDifferences)
            || string.IsNullOrWhiteSpace(questionnaireData.PsyQuery)
            || string.IsNullOrWhiteSpace(questionnaireData.TherapyExperience))
        {
            throw new IncorrectDataException(
                "Name, Pronouns, Time zone, Neuro differences, Psy query, Therapy experience values cannot be null or empty");
        }
        
        if (!string.IsNullOrWhiteSpace(questionnaireData.Email))
        {
            if (!Validator.EmailValidator(questionnaireData.Email))
                throw new IncorrectDataException("Incorrect email address format");
        }

        if (!string.IsNullOrWhiteSpace(questionnaireData.Phone))
        {
            if (!Validator.PhoneNumberValidator(questionnaireData.Phone))
                throw new IncorrectDataException("Incorrect phone number format");
        }

        if (questionnaireData.Age < 16)
            throw new IncorrectDataException("Age value must be at least 16");

        var contactData = new Contact()
        {
            Email = questionnaireData.Email,
            Phone = questionnaireData.Phone,
            Telegram = questionnaireData.Telegram
        };

        var contact = await _contactRepository.AddAsync(contactData, cancellationToken);

        var createdQuestionnaire = _applicationMapper.Map<Questionnaire>(questionnaireData);
        createdQuestionnaire.ContactId = contact.Id;
        createdQuestionnaire.RegistrationDate = DateTime.Now.ToUniversalTime();
        
        _memoryCache.Remove(string.Format(QuestionnaireCacheName, "All"));

        return _applicationMapper.Map<QuestionnaireDto>(
            await _questionnaireRepository.AddAsync(createdQuestionnaire, cancellationToken));
    }
}