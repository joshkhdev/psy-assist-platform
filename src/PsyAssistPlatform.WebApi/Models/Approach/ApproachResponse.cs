namespace PsyAssistPlatform.WebApi.Models.Approach;

public record ApproachResponse
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}