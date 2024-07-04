namespace PsyAssistPlatform.WebApi.Models.Questionnaire;

public record QuestionnaireResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Pronouns { get; set; } = null!;
    
    public int Age { get; set; }
    
    public string TimeZone { get; set; } = null!;
    
    public string? Telegram { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
    
    public string NeuroDifferences { get; set; } = null!;
    
    public string? MentalSpecifics { get; set; }
    
    public string? PsyWishes { get; set; }
    
    public string PsyQuery { get; set; } = null!;
    
    public string TherapyExperience { get; set; } = null!;
    
    public bool IsForPay { get; set; }
    
    public DateTime RegistrationDate { get; set; }
}