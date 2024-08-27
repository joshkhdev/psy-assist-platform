using PsyAssistPlatform.WebApi.Models.Status;

namespace PsyAssistPlatform.WebApi.Models.PsyRequest;

public record PsyRequestStatusResponse
{
    public StatusResponse Status { get; set; } = null!;
    
    public DateTime StatusUpdateDate { get; set; }
    
    public string? Comment { get; set; }
}