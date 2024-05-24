namespace PsyAssistPlatform.Domain;

/// <summary>
/// Анкета
/// </summary>
public class Questionnaire : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Pronouns { get; set; } = null!;

    public int Age { get; set; }

    public string TimeZone { get; set; } = null!;
    
    public int ContactId { get; set; }
    
    public virtual Contact Contact { get; set; } = null!;

    public string NeuroDifferences { get; set; } = null!;

    public string MentalSpecifics { get; set; } = null!;

    public string PsyWishes { get; set; } = null!;

    public string PsyRequest { get; set; } = null!;

    public string TherapyExperience { get; set; } = null!;

    public bool IsForPay { get; set; }

    public DateTime RegistrationDate { get; set; }
}