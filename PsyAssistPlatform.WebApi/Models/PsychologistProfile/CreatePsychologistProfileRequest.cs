using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;

namespace PsyAssistPlatform.WebApi.Models.PsychologistProfile;

public record CreatePsychologistProfileRequest : ICreatePsychologistProfile
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string TimeZone { get; set; }

    public required string RequestsInclude { get; set; }

    public required string RequestsExclude { get; set; }

    public required int UserId { get; set; }
}