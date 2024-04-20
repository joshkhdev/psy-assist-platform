using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.User;
using System.Linq.Expressions;

namespace PsyAssistPlatform.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UsersController(
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetActiveUsers(CancellationToken cancellationToken)
    {
        Expression<Func<User, bool>> getActiveUsers = (user) => !user.IsBlocked;

        var activeUsers = await _userRepository.GetAsync(getActiveUsers, cancellationToken);

        var activeUsersResponse = _mapper.Map<IEnumerable<UserResponse>>(activeUsers);

        return Ok(activeUsersResponse);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        var usersResponse = _mapper.Map<IEnumerable<UserResponse>>(users);

        return Ok(usersResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest createRequest, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(createRequest);
        user.IsBlocked = false;

        await _userRepository.AddAsync(user, cancellationToken);

        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return NotFound();

        var userResponse = _mapper.Map<UserResponse>(user);

        return Ok(userResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest updateRequest, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return NotFound();

        var userModel = _mapper.Map<User>(updateRequest);
        userModel.Id = id;

        await _userRepository.UpdateAsync(userModel, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> BlockUser(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return NotFound();

        user.IsBlocked = true;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Ok();
    }
}
