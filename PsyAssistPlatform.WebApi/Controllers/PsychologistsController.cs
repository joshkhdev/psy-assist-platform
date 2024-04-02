using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Psychologist;

namespace PsyAssistPlatform.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PsychologistsController : ControllerBase
{
    private readonly IRepository<Psychologist> _psychologistRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public PsychologistsController(IRepository<Psychologist> psychologistRepository,
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _psychologistRepository = psychologistRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение списка всех психологов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<PsychologistShortResponse>> GetAllAsync(
        CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<PsychologistShortResponse>>(
            await _psychologistRepository.GetAllAsync(cancellationToken));

    /// <summary>
    /// Получение психолога по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
        => Ok(_mapper.Map<PsychologistResponse>(
            await _psychologistRepository.GetByIdAsync(id, cancellationToken)));

    /// <summary>
    /// Создание нового психолога
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        CreatePsychologistRequest request,
        CancellationToken cancellationToken)
    {
        // TODO: ? Handle User Creation ?
        var user = await _userRepository
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return NotFound();

        var psychologist = _mapper.Map<Psychologist>(request);
        psychologist.User = user;

        await _psychologistRepository.AddAsync(psychologist, cancellationToken);

        return Ok(new { id = psychologist.Id });
    }

    /// <summary>
    /// Обновление данных о психологе
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        UpdatePsychologistRequest request,
        CancellationToken cancellationToken)
    {
        // TODO: ? Handle User Creation ?
        var user = await _userRepository
            .GetByIdAsync(request.UserId, cancellationToken);
        var psychologist =
            await _psychologistRepository.GetByIdAsync(id, cancellationToken);

        if (psychologist == null || user == null)
            return NotFound();

        var psychologistModel = _mapper.Map<Psychologist>(request);
        psychologistModel.Id = psychologist.Id;
        psychologistModel.User = user;

        await _psychologistRepository.UpdateAsync(psychologistModel, cancellationToken);

        return Ok(new { id = psychologistModel.Id });
    }

    /// <summary>
    /// Удаление данных о психологе
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var psychologist =
            await _psychologistRepository.GetByIdAsync(id, cancellationToken);

        if (psychologist == null)
            return NotFound();

        await _psychologistRepository.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}
