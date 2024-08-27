using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;
using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;

namespace PsyAssistPlatform.Application.Dto.PsyRequest;

public record PsyRequestInfoDto : IPsyRequestInfo
{
    public int Id { get; set; }

    public IQuestionnaire Questionnaire { get; set; } = null!;
    
    public IPsychologistProfile? PsychologistProfile { get; set; }

    public IPsyRequestStatus PsyRequestStatus { get; set; } = null!;
}