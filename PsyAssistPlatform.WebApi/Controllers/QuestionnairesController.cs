using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.Questionnaire;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionnairesController : ControllerBase
{
    private readonly IQuestionnaireService _questionnaireService;
    private readonly IMapper _mapper;

    public QuestionnairesController(IQuestionnaireService questionnaireService, IMapper mapper)
    {
        _questionnaireService = questionnaireService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список всех анкет
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<QuestionnaireResponse>> GetAllQuestionnairesAsync(CancellationToken cancellationToken)
    {
        var questionnaires = await _questionnaireService.GetQuestionnairesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<QuestionnaireResponse>>(questionnaires);
    }

    /// <summary>
    /// Получить анкету по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionnaireResponse>> GetQuestionnaireByIdAsync(int id, CancellationToken cancellationToken)
    {
        var questionnaire = await _questionnaireService.GetQuestionnaireByIdAsync(id, cancellationToken);
        return _mapper.Map<QuestionnaireResponse>(questionnaire);
    }

    /// <summary>
    /// Создать новую анкету
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuestionnaireAsync(CreateQuestionnaireRequest request, CancellationToken cancellationToken)
    {
        await _questionnaireService.CreateQuestionnaireAsync(request, cancellationToken);
        return Ok();
    }
}
