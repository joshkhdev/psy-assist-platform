using PsyAssistPlatform.Application.Interfaces.Dto.Role;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IRoleService
{
    Task<IEnumerable<IRole>?> GetRolesAsync(CancellationToken cancellationToken);
    
    Task<IRole?> GetRoleByIdAsync(int id, CancellationToken cancellationToken);
}