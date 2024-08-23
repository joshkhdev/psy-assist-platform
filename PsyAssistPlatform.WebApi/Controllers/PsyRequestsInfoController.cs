using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.PsyRequest;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PsyRequestsInfoController : ControllerBase
{
    private readonly IPsyRequestInfoService _psyRequestInfoService;
    private readonly IMapper _mapper;

    public PsyRequestsInfoController(IPsyRequestInfoService psyRequestInfoService, IMapper mapper)
    {
        _psyRequestInfoService = psyRequestInfoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список всех заявок
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<PsyRequestInfoResponse>?> GetAllPsyRequestsAsync(CancellationToken cancellationToken)
    {
        var psyRequests = await _psyRequestInfoService.GetAllPsyRequestsInfoAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PsyRequestInfoResponse>>(psyRequests);
    }

    /// <summary>
    /// Получить список заявок по статусу
    /// </summary>
    [HttpGet("status/{statusId:int}")]
    public async Task<IEnumerable<PsyRequestInfoResponse>?> GetPsyRequestsByStatusIdAsync(
        int statusId,
        CancellationToken cancellationToken)
    {
        var psyRequests = await _psyRequestInfoService.GetPsyRequestsInfoByStatusIdAsync(statusId, cancellationToken);
        return _mapper.Map<IEnumerable<PsyRequestInfoResponse>>(psyRequests);
    }

    /// <summary>
    /// Получить историю изменений данных по заявке
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IEnumerable<PsyRequestInfoResponse>> GetPsyRequestInfoByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var psyRequests = await _psyRequestInfoService.GetPsyRequestInfoByIdAsync(id, cancellationToken);
        return _mapper.Map<IEnumerable<PsyRequestInfoResponse>>(psyRequests);
    }

    /// <summary>
    /// Изменить статус заявки
    /// </summary>
    [HttpPost]
    public async Task<PsyRequestInfoResponse?> ChangePsyRequestStatusAsync(
        ChangePsyRequestStatusRequest changePsyRequestStatusRequest,
        CancellationToken cancellationToken)
    {
        var psyRequestStatus =
            await _psyRequestInfoService.ChangePsyRequestStatusAsync(changePsyRequestStatusRequest, cancellationToken);
        return _mapper.Map<PsyRequestInfoResponse>(psyRequestStatus);
    }
}