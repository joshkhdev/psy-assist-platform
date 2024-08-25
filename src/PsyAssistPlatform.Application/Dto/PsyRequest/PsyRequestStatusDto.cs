using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;
using PsyAssistPlatform.Application.Interfaces.Dto.Status;

namespace PsyAssistPlatform.Application.Dto.PsyRequest;

public record PsyRequestStatusDto : IPsyRequestStatus
{
    public int PsyRequestId { get; set; }
    
    public IStatus Status { get; set; } = null!;
    
    public DateTime StatusUpdateDate { get; set; }

    public string Comment { get; set; } = null!;
}