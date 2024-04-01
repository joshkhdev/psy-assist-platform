namespace PsyAssistPlatform.WebApi.Models.Psychologist;

public class PsychologistShortResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string TimeZone { get; set; }

    public string RequestsInclude { get; set; }

    public string RequestsExclude { get; set; }

    public int UserId { get; set; }
    
    public bool IsActive { get; set; }
}