using PsyAssistPlatform.Application.Interfaces.Dto.Status;

namespace PsyAssistPlatform.Application.Dto.Status;

public record StatusDto : IStatus
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}