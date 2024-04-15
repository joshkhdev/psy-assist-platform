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
        public async Task<IEnumerable<QuestionnaireShortResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<QuestionnaireShortResponse>>(await _questionnaireRepository.GetAllAsync(cancellationToken));
        }


        ///<summary>
        ///Получение анкеты по id
        ///</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<QuestionnaireResponse>(await _questionnaireRepository.GetByIdAsync(id, cancellationToken)));
        }

        ///<summary>
        ///Создание новой анкеты
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateQuestionnaireRequest request, CancellationToken cancellationToken)
        {
            //TODO: create new contact
            var questionnaire = _mapper.Map<Questionnaire>(request);
            await _questionnaireRepository.AddAsync(questionnaire, cancellationToken);
            return Ok(questionnaire.Id);
        }
    }
}
