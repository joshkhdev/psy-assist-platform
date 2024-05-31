namespace PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;

public interface IQuestionnaire
{
    int Id { get; set; }
    
    string Name { get; set; }
    
    string Pronouns { get; set; }
    
    int Age { get; set; }
    
    string TimeZone { get; set; }

    string? Telegram { get; set; }
    
    string? Email { get; set; }
    
    string? Phone { get; set; }
    
    string NeuroDifferences { get; set; }
    
    string? MentalSpecifics { get; set; }
    
    string? PsyWishes { get; set; }
    
    string PsyQuery { get; set; }
    
    string TherapyExperience { get; set; }
    
    bool IsForPay { get; set; }
    
    DateTime RegistrationDate { get; set; }
}