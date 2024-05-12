using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PsychologistProfilesController : ControllerBase
{
    private readonly IPsychologistProfileService _psychologistProfileService;
    private readonly IMapper _mapper;

    public PsychologistProfilesController(IPsychologistProfileService psychologistProfileService, IMapper mapper)
    {
        _psychologistProfileService = psychologistProfileService;
        _mapper = mapper;
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
        var psychologistProfile =
            await _psychologistProfileService.CreatePsychologistProfileAsync(request, cancellationToken);

        return Ok(psychologistProfile);
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
        var psychologistProfile =
            await _psychologistProfileService.UpdatePsychologistProfileAsync(id, request, cancellationToken);

        return Ok(psychologistProfile);
    }

    /// <summary>
    /// Изменить доступность профиля психолога
    /// </summary>
    [HttpPut("{id:int}/{isActive:bool}")]
    public async Task<IActionResult> ChangeAvailabilityPsychologistProfileAsync(
        int id, 
        bool isActive, 
        CancellationToken cancellationToken)
    {
        var psychologistProfile =
            await _psychologistProfileService.ChangeAvailabilityPsychologistProfileAsync(id, isActive, 
                cancellationToken);

        return Ok(psychologistProfile);
    }

    /// <summary>
    /// Деактивировать профиль психолога
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile =
            await _psychologistProfileService.DeactivatePsychologistProfileAsync(id, cancellationToken);
            
        return Ok(psychologistProfile);
    }
}