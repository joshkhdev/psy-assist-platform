using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Integration;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PsychologistProfilesController : ControllerBase
{
    private readonly IPsychologistProfileService _psychologistProfileService;
    private readonly IMapper _mapper;
    private readonly IContentService _contentService;

    public PsychologistProfilesController(IPsychologistProfileService psychologistProfileService, IMapper mapper, IContentService contentService)
    {
        _psychologistProfileService = psychologistProfileService;
        _mapper = mapper;
        _contentService = contentService;
    }

    /// <summary>
    /// Получить список активных профилей психологов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<PsychologistProfileResponse>> GetActivePsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var activePsychologistProfiles = await _psychologistProfileService.GetActivePsychologistProfilesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PsychologistProfileResponse>>(activePsychologistProfiles);
    }

    /// <summary>
    /// Получить список всех профилей психологов
    /// </summary>
    [HttpGet("all")]
    public async Task<IEnumerable<PsychologistProfileResponse>> GetPsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var psychologistProfiles = await _psychologistProfileService.GetAllPsychologistProfilesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PsychologistProfileResponse>>(psychologistProfiles);
    }

    /// <summary>
    /// Получить профиль психолога по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PsychologistProfileResponse>> GetPsychologistProfileByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var psychologistProfile =
            await _psychologistProfileService.GetPsychologistProfileByIdAsync(id, cancellationToken);

        return _mapper.Map<PsychologistProfileResponse>(psychologistProfile);
    }

    /// <summary>
    /// Создать новый профиль психолога
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreatePsychologistProfileAsync(
        CreatePsychologistProfileRequest request,
        CancellationToken cancellationToken)
    {
        await _psychologistProfileService.CreatePsychologistProfileAsync(request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Обновить данные профиля психолога
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePsychologistProfileAsync(
        int id,
        UpdatePsychologistProfileRequest request,
        CancellationToken cancellationToken)
    {
        await _psychologistProfileService.UpdatePsychologistProfileAsync(id, request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Активировать профиль психолога
    /// </summary>
    [HttpPut("{id:int}/activate")]
    public async Task<IActionResult> ActivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        await _psychologistProfileService.ActivatePsychologistProfileAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Деактивировать профиль психолога
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        await _psychologistProfileService.DeactivatePsychologistProfileAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Получить контент психолога
    /// </summary>
    [HttpGet("{id:int}/content")]
    public async Task<IActionResult> GetPsychologistContentAsync(int id, int type, CancellationToken cancellationToken)
    {
        var response = await _contentService.GetContentAsync(id, type, cancellationToken);
        return Ok(response);
    }
}