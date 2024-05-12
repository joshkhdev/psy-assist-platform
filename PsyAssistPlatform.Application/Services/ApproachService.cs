using AutoMapper;
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
    
    public ApproachService(IRepository<Approach> approachRepository, IMapper applicationMapper)
    {
        _approachRepository = approachRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IApproach>> GetApproachesAsync(CancellationToken cancellationToken)
    {
        var approaches = await _approachRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<ApproachDto>>(approaches);
    }

    public async Task<IApproach?> GetApproachByIdAsync(int id, CancellationToken cancellationToken)
    {
        var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);
        if (approach is null)
            throw new NotFoundException($"Approach with Id [{id}] not found");

        return _applicationMapper.Map<ApproachDto>(approach);
    }

    public async Task<IApproach> CreateApproachAsync(ICreateApproach approachData, CancellationToken cancellationToken)
    {
        var approach = _applicationMapper.Map<Approach>(approachData);

        return _applicationMapper.Map<ApproachDto>(await _approachRepository.AddAsync(approach, cancellationToken));
    }

    public async Task<IApproach> UpdateApproachAsync(int id, IUpdateApproach approachData, CancellationToken cancellationToken)
    {
        var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);
        if (approach is null)
            throw new NotFoundException($"Approach with Id [{id}] not found");

        var updatedApproach = _applicationMapper.Map<Approach>(approachData);
        updatedApproach.Id = approach.Id;

        return _applicationMapper.Map<ApproachDto>(await _approachRepository.UpdateAsync(updatedApproach, cancellationToken));
    }

    public async Task DeleteApproachAsync(int id, CancellationToken cancellationToken)
    {
        var approach = await _approachRepository.GetByIdAsync(id, cancellationToken);
        if (approach is null)
            throw new NotFoundException($"Approach with Id [{id}] not found");

        await _approachRepository.DeleteAsync(id, cancellationToken);
    }
}