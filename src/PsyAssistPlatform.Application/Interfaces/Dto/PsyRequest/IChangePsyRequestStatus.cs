namespace PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

public interface IChangePsyRequestStatus
{
    int PsyRequestId { get; set; }
    
    int NewStatusId { get; set; }
    
    int PsychologistProfileId { get; set; }
    
    string Comment { get; set; }
}