using AutoMapper;
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

    public QuestionnaireService(
        IRepository<Questionnaire> questionnaireRepository, 
        IRepository<Contact> contactRepository, 
        IMapper applicationMapper)
    {
        _questionnaireRepository = questionnaireRepository;
        _contactRepository = contactRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IQuestionnaire>> GetQuestionnairesAsync(CancellationToken cancellationToken)
    {
        var questionnaires = await _questionnaireRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<QuestionnaireDto>>(questionnaires);
    }

    public async Task<IQuestionnaire?> GetQuestionnaireByIdAsync(int id, CancellationToken cancellationToken)
    {
        var questionnaire = await _questionnaireRepository.GetByIdAsync(id, cancellationToken);
        if (questionnaire is null)
            throw new NotFoundException($"Questionnaire with Id [{id}] not found");

        return _applicationMapper.Map<QuestionnaireDto>(questionnaire);
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

        if (questionnaireData.Age < 16)
            throw new IncorrectDataException("Age value must be at least 16");

        var contactData = new Contact()
        {
            Email = questionnaireData.Email,
            Phone = questionnaireData.Phone,
            Telegram = questionnaireData.Telegram
        };

        var contact = await _contactRepository.AddAsync(contactData, cancellationToken);

        var createdQuestionnaire =
            _applicationMapper.Map<Questionnaire>(questionnaireData);
        createdQuestionnaire.ContactId = contact.Id;
        createdQuestionnaire.RegistrationDate = DateTime.Now;

        return _applicationMapper.Map<QuestionnaireDto>(
            await _questionnaireRepository.AddAsync(createdQuestionnaire, cancellationToken));
    }
}