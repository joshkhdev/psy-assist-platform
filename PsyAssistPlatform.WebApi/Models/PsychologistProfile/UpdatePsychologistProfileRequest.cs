using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;

namespace PsyAssistPlatform.WebApi.Models.PsychologistProfile;

public record UpdatePsychologistProfileRequest : IUpdatePsychologistProfile
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string TimeZone { get; set; }

    public required string IncludingQueries { get; set; }

    public required string ExcludingQueries { get; set; }

    public required int UserId { get; set; }

    public required bool IsActive { get; set; }
}