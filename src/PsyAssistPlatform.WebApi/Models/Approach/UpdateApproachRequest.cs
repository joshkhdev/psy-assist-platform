using PsyAssistPlatform.Application.Interfaces.Dto.Approach;

namespace PsyAssistPlatform.WebApi.Models.Approach;

public record UpdateApproachRequest : IUpdateApproach
{
    public required string Name { get; set; }
}