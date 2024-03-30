namespace PsyAssistPlatform.Domain;

/// <summary>
/// Пользователь
/// </summary>
public class User : BaseEntity
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }

    public bool IsBlocked { get; set; }
    
    public int RoleId { get; set; }
    
    public virtual Role Role { get; set; }
}

