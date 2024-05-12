﻿namespace PsyAssistPlatform.Domain;

/// <summary>
/// Профиль психолога
/// </summary>
public class PsychologistProfile : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    public string RequestsInclude { get; set; } = null!;

    public string RequestsExclude { get; set; } = null!;

    public int UserId { get; set; }
    
    public virtual User User { get; set; } = null!;

    public bool IsActive { get; set; }
}