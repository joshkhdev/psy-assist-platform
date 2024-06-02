namespace PsyAssistPlatform.WebApi.Models.PsychologistProfile;

public record PsychologistProfileResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    public string IncludingQueries { get; set; } = null!;

    public string ExcludingQueries { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsActive { get; set; }
}