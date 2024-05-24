using PsyAssistPlatform.WebApi.Models.Role;

namespace PsyAssistPlatform.WebApi.Models.User;

public record UserResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsBlocked { get; set; }

    public int RoleId { get; set; }
}