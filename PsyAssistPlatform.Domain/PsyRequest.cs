namespace PsyAssistPlatform.Domain;

/// <summary>
/// Заявка
/// </summary>
public class PsyRequest : BaseEntity
{
    public int QuestionnaireId { get; set; }

    public virtual Questionnaire Questionnaire { get; set; } = null!;
    
    public int? PsychologistProfileId { get; set; }
    
    public virtual PsychologistProfile? PsychologistProfile { get; set; }

    public virtual ICollection<PsyRequestStatus> PsyRequestStatuses { get; set; } = new List<PsyRequestStatus>();
}