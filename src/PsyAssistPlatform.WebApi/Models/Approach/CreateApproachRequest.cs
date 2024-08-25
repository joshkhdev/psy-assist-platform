using PsyAssistPlatform.Application.Interfaces.Dto.Approach;

namespace PsyAssistPlatform.WebApi.Models.Approach;

public record CreateApproachRequest : ICreateApproach
{
    public required string Name { get; set; }
}