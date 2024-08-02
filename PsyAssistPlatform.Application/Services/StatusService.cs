using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Dto.Status;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.Status;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class StatusService : IStatusService
{
    private readonly IRepository<Status> _statusRepository;
    private readonly IMapper _applicationMapper;
    private readonly IMemoryCache _memoryCache;
    private const string StatusCacheName = "Status_{0}";

    public StatusService(IRepository<Status> statusRepository, IMapper applicationMapper, IMemoryCache memoryCache)
    {
        _statusRepository = statusRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IStatus>?> GetStatusesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(StatusCacheName, "All");
        var statuses = await _memoryCache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var allStatuses = await _statusRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<StatusDto>>(allStatuses);
        });

        return statuses;
    }

    public async Task<IStatus?> GetStatusByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(StatusCacheName, id);
        var status = await _memoryCache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var statusById = await _statusRepository.GetByIdAsync(id, cancellationToken);
            if (statusById is null)
                throw new NotFoundException($"Status with Id [{id}] not found");

            return _applicationMapper.Map<StatusDto>(statusById);
        });

        return status;
    }
}