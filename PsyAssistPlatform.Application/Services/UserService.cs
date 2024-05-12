using AutoMapper;
using PsyAssistPlatform.Application.Dto.User;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.User;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<PsychologistProfile> _psychologistProfileRepository;
    private readonly IMapper _applicationMapper;

    public UserService(
        IRepository<User> userRepository, 
        IRepository<PsychologistProfile> psychologistProfileRepository, 
        IMapper applicationMapper)
    {
        _userRepository = userRepository;
        _psychologistProfileRepository = psychologistProfileRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IUser>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<IEnumerable<IUser>> GetActiveUsersAsync(CancellationToken cancellationToken)
    {
        var activeUsers = await _userRepository.GetAsync(user => user.IsBlocked == false, cancellationToken);
        return _applicationMapper.Map<IEnumerable<UserDto>>(activeUsers);
    }

    public async Task<IUser?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new NotFoundException($"User with Id [{id}] not found");

        return _applicationMapper.Map<UserDto>(user);
    }

    public async Task<IUser> CreateUserAsync(ICreateUser userData, CancellationToken cancellationToken)
    {
        var user = _applicationMapper.Map<User>(userData);

        return _applicationMapper.Map<UserDto>(await _userRepository.AddAsync(user, cancellationToken));
    }

    public async Task<IUser> UpdateUserAsync(int id, IUpdateUser userData, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new NotFoundException($"User with Id [{id}] not found");

        if (user.RoleId != userData.RoleId && userData.RoleId == (int) RoleType.Admin)
        {
            var psychologistProfiles =
                await _psychologistProfileRepository.GetAsync(profile => profile.UserId == id, cancellationToken);
            if (psychologistProfiles.Any())
                throw new IncorrectDataException("User cannot be an admin, because he has a psychologist's profile");
        }

        var updatedUser = _applicationMapper.Map<User>(userData);
        updatedUser.Id = id;
        
        return _applicationMapper.Map<UserDto>(await _userRepository.UpdateAsync(updatedUser, cancellationToken));
    }

    public async Task BlockUserAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new NotFoundException($"User with Id [{id}] not found");

        user.IsBlocked = true;
        
        var psychologistProfiles =
            await _psychologistProfileRepository.GetAsync(profile => profile.UserId == id, cancellationToken);
        
        var profilesList = psychologistProfiles.ToList();
        if (profilesList.Count > 0)
        {
            var psychologistProfile = profilesList.Single();
            psychologistProfile.IsActive = false;

            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}