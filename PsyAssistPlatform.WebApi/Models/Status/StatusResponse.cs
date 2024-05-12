namespace PsyAssistPlatform.WebApi.Models.Status;

public record StatusResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
}
