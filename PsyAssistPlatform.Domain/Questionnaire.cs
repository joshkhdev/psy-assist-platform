﻿namespace PsyAssistPlatform.Domain;

/// <summary>
/// Анкета
/// </summary>
public class Questionnaire : BaseEntity
{
    public string Name { get; set; }

    public string Pronouns { get; set; }

    public int Age { get; set; }

    public string TimeZone { get; set; }

    public int ContactId { get; set; }
    
    public virtual Contact Contact { get; set; }

    public string NeuroDifferences { get; set; }

    public string MentalSpecifics { get; set; }

    public string PsyWishes { get; set; }

    public string PsyRequest { get; set; }

    public string TherapyExperience { get; set; }

    public bool IsForPay { get; set; }

    public DateTime RegistrationDate { get; set; }
}
