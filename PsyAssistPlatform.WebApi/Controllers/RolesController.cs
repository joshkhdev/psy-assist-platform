using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Role;
using PsyAssistPlatform.WebApi.Models.Status;

namespace PsyAssistPlatform.WebApi.Controllers;

/// <summary>
/// Роли пользователей
/// </summary>
[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IMapper _mapper;
    public RolesController(IRepository<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список ролей
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<RoleResponse>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleResponse>>(roles);
    }

    /// <summary>
    /// Получить роль по id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponse>> GetRoleAsync(int id, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(id, cancellationToken);

        if (role == null)
            return NotFound($"Role {id} not found");

        return Ok(_mapper.Map<RoleResponse>(role));
    }

    /// <summary>
    /// Добавить роль
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            return BadRequest("Request is empty");

        var role = _mapper.Map<Role>(request);

        await _roleRepository.AddAsync(role, cancellationToken);

        return Ok(_mapper.Map<RoleResponse>(role));
    }

    /// <summary>
    /// Обновить роль
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoleAsync(int id, UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            return BadRequest("Request is empty");

        var role = await _roleRepository.GetByIdAsync(id, cancellationToken);

        if (role == null)
            return NotFound($"Role {id} not found");

        var roleUpdate = _mapper.Map<Role>(request);
        roleUpdate.Id = role.Id;

        await _roleRepository.UpdateAsync(roleUpdate, cancellationToken);

        return Ok(_mapper.Map<RoleResponse>(roleUpdate));
    }

    /// <summary>
    /// Удалить роль
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteRoleAsync(int id, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(id, cancellationToken);

        if (role == null)
            return NotFound($"Role {id} not found");

        await _roleRepository.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}
