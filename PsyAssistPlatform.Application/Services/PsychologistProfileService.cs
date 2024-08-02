using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    
    private const string PsychologistProfileNotFoundMessage = "Psychologist profile with Id [{0}] not found";
    private const string UserNotFoundMessage = "User with Id [{0}] not found";
    private const string UserCannotBePsychologistMessage = "This user cannot be a psychologist";
    private const string UserCorrespondingToPsychologistProfileNotFoundMessage =
        "User with Id [{0}] corresponding to psychologist profile with Id [{1}] not found";
    private const string ValuesCannotBeMessage =
        "Name, Description, Time zone, Including queries, Excluding queries values cannot be null or empty";
    private const string PsychologistProfileCannotBeAccessibleMessage =
        "Psychologist profile with Id [{0}] cannot be accessible, because the user is blocked";
    private const string PsychologistProfileCacheName = "PsychologistProfile_{0}";

    public PsychologistProfileService(
        IRepository<PsychologistProfile> psychologistProfileRepository, 
        IRepository<User> userRepository,
        IMapper applicationMapper,
        IMemoryCache memoryCache)
    {
        _psychologistProfileRepository = psychologistProfileRepository;
        _userRepository = userRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IPsychologistProfile>?> GetAllPsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(PsychologistProfileCacheName, "All");
        var psychologistProfiles = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(12));

            var allPsychologistProfiles = await _psychologistProfileRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<PsychologistProfileDto>>(allPsychologistProfiles);
        });

        return psychologistProfiles;
    }

    public async Task<IEnumerable<IPsychologistProfile>?> GetActivePsychologistProfilesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(PsychologistProfileCacheName, "Active");
        var activePsychologistProfiles = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(12));

            var allActivePsychologistProfiles =
                await _psychologistProfileRepository.GetAsync(profile => profile.IsActive == true, cancellationToken);
            return _applicationMapper.Map<IEnumerable<PsychologistProfileDto>>(allActivePsychologistProfiles);
        });

        return activePsychologistProfiles;
    }

    public async Task<IPsychologistProfile?> GetPsychologistProfileByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(PsychologistProfileCacheName, id);
        var psychologistProfile = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(12));
            
            var psychologistProfileById = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
            if (psychologistProfileById is null)
                throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
            
            return _applicationMapper.Map<PsychologistProfileDto>(psychologistProfileById);
        });

        return psychologistProfile;
    }

    public async Task<IPsychologistProfile> CreatePsychologistProfileAsync(
        ICreatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(psychologistProfileData.Name)
            || string.IsNullOrWhiteSpace(psychologistProfileData.Description)
            || string.IsNullOrWhiteSpace(psychologistProfileData.TimeZone)
            || string.IsNullOrWhiteSpace(psychologistProfileData.IncludingQueries)
            || string.IsNullOrWhiteSpace(psychologistProfileData.ExcludingQueries))
        {
            throw new IncorrectDataException(ValuesCannotBeMessage);
        }
        
        var user = await _userRepository.GetByIdAsync(psychologistProfileData.UserId, cancellationToken);
        if (user is null)
            throw new IncorrectDataException(string.Format(UserNotFoundMessage, psychologistProfileData.UserId));
        if ((RoleType)user.RoleId == RoleType.Admin)
            throw new IncorrectDataException(UserCannotBePsychologistMessage);
        
        var psychologistProfile =
            _applicationMapper.Map<PsychologistProfile>(psychologistProfileData);
        psychologistProfile.IsActive = true;
        
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "All"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "Active"));

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.AddAsync(psychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> UpdatePsychologistProfileAsync(
        int id, 
        IUpdatePsychologistProfile psychologistProfileData,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(psychologistProfileData.Name)
            || string.IsNullOrWhiteSpace(psychologistProfileData.Description)
            || string.IsNullOrWhiteSpace(psychologistProfileData.TimeZone)
            || string.IsNullOrWhiteSpace(psychologistProfileData.IncludingQueries)
            || string.IsNullOrWhiteSpace(psychologistProfileData.ExcludingQueries))
        {
            throw new IncorrectDataException(ValuesCannotBeMessage);
        }
        
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
        updatedPsychologistProfile.IsActive = psychologistProfile.IsActive;
        
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "All"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "Active"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, id));

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(updatedPsychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> ActivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
        
        if (psychologistProfile.IsActive)
            return _applicationMapper.Map<PsychologistProfileDto>(psychologistProfile);

        var user = await _userRepository.GetByIdAsync(psychologistProfile.UserId, cancellationToken);
        if (user is null)
        {
            throw new InternalPlatformErrorException(
                string.Format(UserCorrespondingToPsychologistProfileNotFoundMessage, psychologistProfile.UserId, id));
        }

        if (user.IsBlocked)
        {
            throw new BusinessLogicException(string.Format(PsychologistProfileCannotBeAccessibleMessage, id));
        }

        psychologistProfile.IsActive = true;
        
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "All"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "Active"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, id));

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }

    public async Task<IPsychologistProfile> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken)
    {
        var psychologistProfile = await _psychologistProfileRepository.GetByIdAsync(id, cancellationToken);
        if (psychologistProfile is null)
            throw new NotFoundException(string.Format(PsychologistProfileNotFoundMessage, id));
        
        if (psychologistProfile.IsActive == false)
            return _applicationMapper.Map<PsychologistProfileDto>(psychologistProfile);
        
        psychologistProfile.IsActive = false;
        
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "All"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, "Active"));
        _memoryCache.Remove(string.Format(PsychologistProfileCacheName, id));

        return _applicationMapper.Map<PsychologistProfileDto>(
            await _psychologistProfileRepository.UpdateAsync(psychologistProfile, cancellationToken));
    }
}