using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;

namespace PsyAssistPlatform.Application.Dto.PsychologistProfile;

public record PsychologistProfileDto : IPsychologistProfile
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string TimeZone { get; set; }

    public required string RequestsInclude { get; set; }

    public required string RequestsExclude { get; set; }

    public required int UserId { get; set; }

    public bool IsActive { get; set; }
}