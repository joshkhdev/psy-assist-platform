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
    private readonly IRepository<User> _useRepository;
    private readonly IMapper _applicationMapper;

    public PsychologistProfileService(
        IRepository<PsychologistProfile> psychologistProfileRepository, 
        IRepository<User> useRepository,
        IMapper applicationMapper)
    {
        _psychologistProfileRepository = psychologistProfileRepository;
        _useRepository = useRepository;
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
            throw new NotFoundException($"Psychologist profile with Id [{id}] not found");

        return _applicationMapper.Map<PsychologistProfileDto>(psychologistProfile);
    }

    public async Task<IPsychologistProfile> CreatePsychologistProfileAsync(
        ICreatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken)
    {
        var user = await _useRepository.GetByIdAsync(psychologistProfileData.UserId, cancellationToken);
        if (user is null)
            throw new IncorrectDataException($"User with Id [{psychologistProfileData.UserId}] not found");
        if ((RoleType)user.RoleId == RoleType.Admin)
            throw new IncorrectDataException("This user cannot be a psychologist");
        
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
            throw new NotFoundException($"Psychologist profile with Id [{id}] not found");
        
        var user = await _useRepository.GetByIdAsync(psychologistProfileData.UserId, cancellationToken);
        if (user is null)
            throw new IncorrectDataException($"User with Id [{psychologistProfileData.UserId}] not found");
        if ((RoleType)user.RoleId == RoleType.Admin)
            throw new IncorrectDataException("This user cannot be a psychologist");

        var updatedPsychologistProfile =
            _applicationMapper.Map<PsychologistProfile>(psychologistProfileData);
        updatedPsychologistProfile.Id = id;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(updatedPsychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> ChangeAvailabilityPsychologistProfileAsync(
        int id, 
        bool isActive, 
        CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException($"Psychologist profile with Id [{id}] not found");

        psychologistProfile.IsActive = isActive;
        
        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException($"Psychologist profile with Id [{id}] not found");
        
        psychologistProfile.IsActive = false;

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }
}