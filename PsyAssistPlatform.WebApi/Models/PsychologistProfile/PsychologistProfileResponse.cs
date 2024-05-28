namespace PsyAssistPlatform.WebApi.Models.PsychologistProfile;

public record PsychologistProfileResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    public string RequestsInclude { get; set; } = null!;

    public string RequestsExclude { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsActive { get; set; }
}