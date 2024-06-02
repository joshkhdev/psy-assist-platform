using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;

namespace PsyAssistPlatform.WebApi.Models.Questionnaire;

public record CreateQuestionnaireRequest : ICreateQuestionnaire
{
    public required string Name { get; set; }
    
    public required string Pronouns { get; set; }
    
    public required int Age { get; set; }
    
    public required string TimeZone { get; set; }

    public string? Telegram { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public required string NeuroDifferences { get; set; }
    
    public string? MentalSpecifics { get; set; }
    
    public string? PsyWishes { get; set; }
    
    public required string PsyQuery { get; set; }
    
    public required string TherapyExperience { get; set; }
    
    public required bool IsForPay { get; set; }
}
