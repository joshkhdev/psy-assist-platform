using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.WebApi.Models.User;
using PsyAssistPlatform.Application.Interfaces.Service;

namespace PsyAssistPlatform.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список действующих пользователей
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<UserResponse>> GetActiveUsersAsync(CancellationToken cancellationToken)
    {
        var activeUsers = await _userService.GetActiveUsersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserResponse>>(activeUsers);
    }

    /// <summary>
    /// Получить список всех пользователей
    /// </summary>
    [HttpGet("all")]
    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var activeUsers = await _userService.GetAllUsersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserResponse>>(activeUsers);
    }
    
    /// <summary>
    /// Получить пользователя по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        return _mapper.Map<UserResponse>(user);
    }
    
    /// <summary>
    /// Создать пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserRequest createRequest, CancellationToken cancellationToken)
    {
        var user = await _userService.CreateUserAsync(createRequest, cancellationToken);
        return Ok(user);
    }

    /// <summary>
    /// Обновить данные пользователя
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UpdateUserRequest updateRequest, CancellationToken cancellationToken)
    {
        var user = await _userService.UpdateUserAsync(id, updateRequest, cancellationToken);
        return Ok(user);
    }

    /// <summary>
    /// Заблокировать пользователя
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> BlockUserAsync(int id, CancellationToken cancellationToken)
    {
        await _userService.BlockUserAsync(id, cancellationToken);
        return Ok();
    }
}
