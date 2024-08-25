using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.Status;

namespace PsyAssistPlatform.WebApi.Controllers;

/// <summary>
/// Статусы заявок
/// </summary>
[ApiController]
[Route("[controller]")]
public class StatusesController : ControllerBase
{
    private readonly IStatusService _statusService;
    private readonly IMapper _mapper;
    
    public StatusesController(IStatusService statusService, IMapper mapper)
    {
        _statusService = statusService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список статусов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<StatusResponse>> GetStatusesAsync(CancellationToken cancellationToken)
    {
        var statuses = await _statusService.GetStatusesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<StatusResponse>>(statuses);
    }

    /// <summary>
    /// Получить статус по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StatusResponse>> GetStatusByIdAsync(int id, CancellationToken cancellationToken)
    {
        var status = await _statusService.GetStatusByIdAsync(id, cancellationToken);
        return _mapper.Map<StatusResponse>(status);
    }
}

