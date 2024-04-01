namespace PsyAssistPlatform.Domain;

/// <summary>
/// Психолог
/// </summary>
public class Psychologist : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public TimeZoneInfo TimeZone { get; set; }

    public string RequestsInclude { get; set; }

    public string RequestsExclude { get; set; }

    public int UserId { get; set; }
    
    public virtual User User { get; set; }

    public bool IsActive { get; set; }
}