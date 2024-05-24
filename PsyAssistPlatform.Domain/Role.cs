namespace PsyAssistPlatform.Domain;

/// <summary>
/// Роль пользователя
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
}