using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Questionnaire;

namespace PsyAssistPlatform.WebApi.Controllers
{
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
        public async Task<IEnumerable<QuestionnaireShortResponse>> GetAllQuestionnaireAsync(CancellationToken cancellationToken)
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
        public async Task<IActionResult> AddQuestionnaireAsync(CreateQuestionnaireRequest request, CancellationToken cancellationToken)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionnaireAsync(int id, CancellationToken cancellationToken)
        {
            var questionnaire = await _questionnaireRepository.GetByIdAsync(id, cancellationToken);
            if (questionnaire == null)
                return NotFound();

            await _contactRepository.DeleteAsync(questionnaire.Contact.Id, cancellationToken);

            await _questionnaireRepository.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
