using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Questionnaire;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionnaireController : ControllerBase
{
    private readonly IRepository<Questionnaire> _questionnaireRepository;
    private readonly IRepository<Contact> _contactRepository;
    private readonly IMapper _mapper;

    public QuestionnaireController(IRepository<Questionnaire> questionnaireRepository, IRepository<Contact> contactRepository, IMapper mapper)
    {
        _questionnaireRepository = questionnaireRepository;
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    ///<summary>
    ///Получение списка всех анкет
    ///</summary>
    [HttpGet]
    public async Task<IEnumerable<QuestionnaireShortResponse>> GetAllQuestionnairesAsync(CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<QuestionnaireShortResponse>>(await _questionnaireRepository.GetAllAsync(cancellationToken));
    }

    ///<summary>
    ///Получение анкеты по id
    ///</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionnaireByIdAsync(int id, CancellationToken cancellationToken)
    {
        return Ok(_mapper.Map<QuestionnaireResponse>(await _questionnaireRepository.GetByIdAsync(id, cancellationToken)));
    }

    ///<summary>
    ///Создание новой анкеты
    ///</summary>
    [HttpPost]
    public async Task<IActionResult> AddQuestionnaireAsync(CreateOrUpdateQuestionnaireRequest request, CancellationToken cancellationToken)
    {
        var contact = new Contact
        {
            Telegram = request.ContactTelegram,
            Email = request.ContactEmail,
            Phone = request.ContactPhone
        };

        await _contactRepository.AddAsync(contact, cancellationToken);

        var questionnaire = _mapper.Map<Questionnaire>(request);
        questionnaire.ContactId = contact.Id; // Устанавливаем ID контакта после его сохранения

        await _questionnaireRepository.AddAsync(questionnaire, cancellationToken);
        return Ok(questionnaire.Id);
    }

    ///<summary>
    ///Обновление анкеты
    ///</summary>   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestionnaireAsync(int id, CreateOrUpdateQuestionnaireRequest request, CancellationToken cancellationToken)
    {
        var questionnaire = await _questionnaireRepository.GetByIdAsync(id, cancellationToken);
        if (questionnaire == null)
            return NotFound();

        var contact = await _contactRepository.GetByIdAsync(questionnaire.ContactId, cancellationToken);
        contact.Telegram = request.ContactTelegram;
        contact.Email = request.ContactEmail;
        contact.Phone = request.ContactPhone;
        await _contactRepository.UpdateAsync(contact, cancellationToken);

        questionnaire = _mapper.Map(request, questionnaire);
        await _questionnaireRepository.UpdateAsync(questionnaire, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestionnaireAsync(int id, CancellationToken cancellationToken)
    {
        var questionnaire = await _questionnaireRepository.GetByIdAsync(id, cancellationToken);
        if (questionnaire == null)
            return NotFound();


        await _questionnaireRepository.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}
