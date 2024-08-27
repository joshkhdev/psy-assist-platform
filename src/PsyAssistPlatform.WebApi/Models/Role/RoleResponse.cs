namespace PsyAssistPlatform.WebApi.Models.Role;

public record RoleResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
}