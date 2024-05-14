using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.Approach;

namespace PsyAssistPlatform.WebApi.Controllers;

/// <summary>
/// Подходы психологов
/// </summary>
[ApiController]
[Route("[controller]")]
public class ApproachesController : ControllerBase
{
    private readonly IApproachService _approachService;
    private readonly IMapper _mapper;
    
    public ApproachesController(IApproachService approachService, IMapper mapper)
    {
        _approachService = approachService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список подходов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<ApproachResponse>> GetApproachesAsync(CancellationToken cancellationToken)
    {
        var approaches = await _approachService.GetApproachesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ApproachResponse>>(approaches);
    }

    /// <summary>
    /// Получить подход по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApproachResponse>> GetApproachByIdAsync(int id, CancellationToken cancellationToken)
    {
        var approach = await _approachService.GetApproachByIdAsync(id, cancellationToken);
        return _mapper.Map<ApproachResponse>(approach);
    }

    /// <summary>
    /// Создать подход
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateApproachAsync(CreateApproachRequest request, CancellationToken cancellationToken)
    {
        await _approachService.CreateApproachAsync(request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Обновить подход
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateApproachAsync(int id, UpdateApproachRequest request, CancellationToken cancellationToken)
    {
        await _approachService.UpdateApproachAsync(id, request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Удалить подход
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteApproachAsync(int id, CancellationToken cancellationToken)
    {
        await _approachService.DeleteApproachAsync(id, cancellationToken);
        return Ok();
    }
}
