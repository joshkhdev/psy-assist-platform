using PsyAssistPlatform.Application.Interfaces.Dto.Approach;

namespace PsyAssistPlatform.Application.Dto.Approach;

public record ApproachDto : IApproach
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}