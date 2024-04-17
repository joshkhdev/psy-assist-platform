namespace PsyAssistPlatform.WebApi.Models.Questionnaire;

public class CreateQuestionnaireRequest
{
    public string Name { get; set; }
    public string Pronouns { get; set; }
    public int Age { get; set; }
    public string TimeZone { get; set; }
    public string ContactTelegram { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string NeuroDifferences { get; set; }
    public string MentalSpecifics { get; set; }
    public string PsyWishes { get; set; }
    public string PsyRequest { get; set; }
    public string TherapyExperience { get; set; }
    public bool IsForPay { get; set; }
    public DateTime RegistrationDate { get; set; }
}