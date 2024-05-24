using PsyAssistPlatform.Application.Interfaces.Dto.Approach;

namespace PsyAssistPlatform.WebApi.Models.Approach;

public record ApproachResponse
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}