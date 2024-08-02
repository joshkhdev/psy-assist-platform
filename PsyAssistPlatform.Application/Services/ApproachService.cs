using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Dto.Approach;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.Approach;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class ApproachService : IApproachService
{
    private readonly IRepository<Approach> _approachRepository;
    private readonly IMapper _applicationMapper;
    private readonly IMemoryCache _memoryCache;
    private const string ApproachNotFoundMessage = "Approach with Id [{0}] not found";
    private const string NameValueCannotMessage = "Name value cannot be null or empty";
    private const string ApproachWithThisNameMessage = "Approach with name [{0}] already exists";
    private const string ApproachCacheName = "Approach_{0}";
    
    public ApproachService(IRepository<Approach> approachRepository, IMapper applicationMapper, IMemoryCache memoryCache)
    {
        _approachRepository = approachRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IApproach>?> GetApproachesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(ApproachCacheName, "All");
        var approaches = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(24));
            
            var allApproaches = await _approachRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<ApproachDto>>(allApproaches);
        });

        return approaches;
    }

    public async Task<IApproach?> GetApproachByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(ApproachCacheName, id);
        var approach = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(24));

            var approachById = await _approachRepository.GetByIdAsync(id, cancellationToken);
            if (approachById is null)
                throw new NotFoundException(string.Format(ApproachNotFoundMessage, id));

            return _applicationMapper.Map<ApproachDto>(approachById);
        });

        return approach;
    }

    public async Task<IApproach> CreateApproachAsync(ICreateApproach approachData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(approachData.Name))
            throw new IncorrectDataException(NameValueCannotMessage);

        var approaches = await _approachRepository.GetAsync(
            approachEntity => approachEntity.Name.ToLower() == approachData.Name.ToLower(), cancellationToken);
        if (approaches.Any())
            throw new IncorrectDataException(string.Format(ApproachWithThisNameMessage, approachData.Name));
        
        var approach = _applicationMapper.Map<Approach>(approachData);
        
        _memoryCache.Remove(string.Format(ApproachCacheName, "All"));

        return _applicationMapper.Map<ApproachDto>(await _approachRepository.AddAsync(approach, cancellationToken));
    }

    public async Task<IApproach> UpdateApproachAsync(int id, IUpdateApproach approachData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(approachData.Name))
            throw new IncorrectDataException(NameValueCannotMessage);
        
        var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);
        if (approach is null)
            throw new NotFoundException(string.Format(ApproachNotFoundMessage, id));

        var approaches = await _approachRepository.GetAsync(
            approachEntity => approachEntity.Name.ToLower() == approachData.Name.ToLower() && approachEntity.Id != id,
            cancellationToken);
        if (approaches.Any())
            throw new IncorrectDataException(string.Format(ApproachWithThisNameMessage, approachData.Name));

        var updatedApproach = _applicationMapper.Map<Approach>(approachData);
        updatedApproach.Id = approach.Id;
        
        _memoryCache.Remove(string.Format(ApproachCacheName, "All"));
        _memoryCache.Remove(string.Format(ApproachCacheName, id));

        return _applicationMapper.Map<ApproachDto>(await _approachRepository.UpdateAsync(updatedApproach, cancellationToken));
    }

    public async Task DeleteApproachAsync(int id, CancellationToken cancellationToken)
    {
        var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);
        if (approach is null)
            throw new NotFoundException(string.Format(ApproachNotFoundMessage, id));
        
        _memoryCache.Remove(string.Format(ApproachCacheName, "All"));
        _memoryCache.Remove(string.Format(ApproachCacheName, id));

        await _approachRepository.DeleteAsync(id, cancellationToken);
    }
}