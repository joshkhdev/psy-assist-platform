using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

namespace PsyAssistPlatform.WebApi.Models.PsyRequest;

public record ChangePsyRequestStatusRequest : IChangePsyRequestStatus
{
    public int PsyRequestId { get; set; }
    
    public int NewStatusId { get; set; }
    
    public int PsychologistProfileId { get; set; }

    public string Comment { get; set; } = null!;
}