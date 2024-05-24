using PsyAssistPlatform.Application.Interfaces.Dto.User;

namespace PsyAssistPlatform.Application.Dto.User;

public record UserDto : IUser
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required bool IsBlocked { get; set; }

    public required int RoleId { get; set; }
}