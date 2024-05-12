namespace PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;

public interface IUpdatePsychologistProfile
{
    string Name { get; set; }

    string Description { get; set; }

    string TimeZone { get; set; }

    string RequestsInclude { get; set; }

    string RequestsExclude { get; set; }

    int UserId { get; set; }

    bool IsActive { get; set; }
}