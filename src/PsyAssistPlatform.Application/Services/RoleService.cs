using AutoMapper;
using PsyAssistPlatform.Application.Dto.Role;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Dto.Role;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IMapper _applicationMapper;

    public RoleService(IRepository<Role> roleRepository, IMapper applicationMapper)
    {
        _roleRepository = roleRepository;
        _applicationMapper = applicationMapper;
    }
    
    public async Task<IEnumerable<IRole>?> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<IRole?> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
        if (role is null)
            throw new NotFoundException($"Role with Id [{id}] not found");
        
        return _applicationMapper.Map<RoleDto>(role);
    }
}