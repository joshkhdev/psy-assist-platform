namespace PsyAssistPlatform.Domain;

/// <summary>
/// Статус заявки
/// </summary>
public class PsyRequestStatus : BaseEntity
{
    public int PsyRequestId { get; set; }

    public virtual PsyRequest PsyRequest { get; set; } = null!;
    
    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;
    
    public DateTime StatusUpdateDate { get; set; }

    public string Comment { get; set; } = null!;
}