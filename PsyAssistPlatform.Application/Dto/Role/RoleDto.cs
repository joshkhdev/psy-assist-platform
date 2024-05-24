using PsyAssistPlatform.Application.Interfaces.Dto.Role;

namespace PsyAssistPlatform.Application.Dto.Role;

public record RoleDto : IRole
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}