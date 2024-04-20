namespace PsyAssistPlatform.WebApi.Models.User;

public record UpdateUserRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsBlocked { get; set; }

    public int RoleId { get; set; }
}
