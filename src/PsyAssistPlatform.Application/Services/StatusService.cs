using AutoMapper;
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

    public StatusService(IRepository<Status> statusRepository, IMapper applicationMapper)
    {
        _statusRepository = statusRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IStatus>?> GetStatusesAsync(CancellationToken cancellationToken)
    {
        var statuses = await _statusRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<StatusDto>>(statuses);
    }

    public async Task<IStatus?> GetStatusByIdAsync(int id, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetByIdAsync(id, cancellationToken);
        if (status is null)
            throw new NotFoundException($"Status with Id [{id}] not found");
        
        return _applicationMapper.Map<StatusDto>(status);
    }
}