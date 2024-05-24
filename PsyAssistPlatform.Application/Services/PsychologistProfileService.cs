using AutoMapper;
using PsyAssistPlatform.Application.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class PsychologistProfileService : IPsychologistProfileService
{
    private readonly IRepository<PsychologistProfile> _psychologistProfileRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _applicationMapper;
    private const string PsychologistProfileNotFoundMessage = "Psychologist profile with Id [{0}] not found";
    private const string UserNotFoundMessage = "User with Id [{0}] not found";
    private const string UserCannotBePsychologistMessage = "This user cannot be a psychologist";

    public PsychologistProfileService(
        IRepository<PsychologistProfile> psychologistProfileRepository, 
        IRepository<User> userRepository,
        IMapper applicationMapper)
    {
        _psychologistProfileRepository = psychologistProfileRepository;
        _userRepository = userRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IPsychologistProfile>> GetAllPsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var psychologistProfiles = await _psychologistProfileRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<PsychologistProfileDto>>(psychologistProfiles);
    }

    public async Task<IEnumerable<IPsychologistProfile>> GetActivePsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var activePsychologistProfiles =
            await _psychologistProfileRepository.GetAsync(profile => profile.IsActive == true, cancellationToken);
        return _applicationMapper.Map<IEnumerable<PsychologistProfileDto>>(activePsychologistProfiles);
    }

    public async Task<IPsychologistProfile?> GetPsychologistProfileByIdAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));

        return _applicationMapper.Map<PsychologistProfileDto>(psychologistProfile);
    }

    public async Task<IPsychologistProfile> CreatePsychologistProfileAsync(
        ICreatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(psychologistProfileData.UserId, cancellationToken);
        if (user is null)
            throw new IncorrectDataException(string.Format(UserNotFoundMessage, psychologistProfileData.UserId));
        if ((RoleType)user.RoleId == RoleType.Admin)
            throw new IncorrectDataException(UserCannotBePsychologistMessage);
        
        var psychologistProfile =
            _applicationMapper.Map<PsychologistProfile>(psychologistProfileData);
        psychologistProfile.IsActive = true;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.AddAsync(psychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> UpdatePsychologistProfileAsync(
        int id, 
        IUpdatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
        
        var user = await _userRepository.GetByIdAsync(psychologistProfileData.UserId, cancellationToken);
        if (user is null)
            throw new IncorrectDataException(string.Format(UserNotFoundMessage, psychologistProfileData.UserId));
        if ((RoleType)user.RoleId == RoleType.Admin)
            throw new IncorrectDataException(UserCannotBePsychologistMessage);

        var updatedPsychologistProfile =
            _applicationMapper.Map<PsychologistProfile>(psychologistProfileData);
        updatedPsychologistProfile.Id = id;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(updatedPsychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> ActivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
        
        psychologistProfile.IsActive = true;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
        
        psychologistProfile.IsActive = false;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }
}