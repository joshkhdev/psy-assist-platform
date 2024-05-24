using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.WebApi.Models.Role;

namespace PsyAssistPlatform.WebApi.Controllers;

/// <summary>
/// Роли пользователей
/// </summary>
[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    
    public RolesController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список ролей
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<RoleResponse>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetRolesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleResponse>>(roles);
    }

    /// <summary>
    /// Получить роль по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleResponse>> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
        return _mapper.Map<RoleResponse>(role);
    }
}
