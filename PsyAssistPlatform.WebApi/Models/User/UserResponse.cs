using PsyAssistPlatform.WebApi.Models.Role;

namespace PsyAssistPlatform.WebApi.Models.User;

public record UserResponse
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsBlocked { get; set; }

    public int RoleId { get; set; }

    public RoleResponse Role { get; set; }
}