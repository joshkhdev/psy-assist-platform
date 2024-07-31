using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    private const string RoleCacheName = "Role_{0}";

    public RoleService(IRepository<Role> roleRepository, IMapper applicationMapper, IMemoryCache memoryCache)
    {
        _roleRepository = roleRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<IRole>?> GetRolesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(RoleCacheName, "All");
        var roles = await _memoryCache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var allRoles = await _roleRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<RoleDto>>(allRoles);
        });

        return roles;
    }

    public async Task<IRole?> GetRoleByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(RoleCacheName, id);
        var role = await _memoryCache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var roleById = await _roleRepository.GetByIdAsync(id, cancellationToken);
            if (roleById is null)
                throw new NotFoundException($"Role with Id [{id}] not found");

            return _applicationMapper.Map<RoleDto>(roleById);
        });

        return role;
    }
}