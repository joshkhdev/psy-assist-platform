using PsyAssistPlatform.Application.Interfaces.Dto.Status;

namespace PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

public interface IPsyRequestStatus
{
    int PsyRequestId { get; set; }
    
    IStatus Status { get; set; }
    
    DateTime StatusUpdateDate { get; set; }
    
    string Comment { get; set; }
}