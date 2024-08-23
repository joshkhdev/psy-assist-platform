using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;

namespace PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

public interface IPsyRequestInfo
{
    int Id { get; set; }
    
    IQuestionnaire Questionnaire { get; set; }
    
    IPsychologistProfile? PsychologistProfile { get; set; }
    
    IPsyRequestStatus PsyRequestStatus { get; set; }
}