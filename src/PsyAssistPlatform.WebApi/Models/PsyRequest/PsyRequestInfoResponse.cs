using PsyAssistPlatform.WebApi.Models.PsychologistProfile;
using PsyAssistPlatform.WebApi.Models.Questionnaire;

namespace PsyAssistPlatform.WebApi.Models.PsyRequest;

public record PsyRequestInfoResponse
{
    public QuestionnaireResponse Questionnaire { get; set; } = null!;
    
    public PsychologistProfileResponse? PsychologistProfile { get; set; }

    public PsyRequestStatusResponse PsyRequestStatus { get; set; } = null!;
}