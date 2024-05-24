using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IPsychologistProfileService
{
    Task<IEnumerable<IPsychologistProfile>> GetActivePsychologistProfilesAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<IPsychologistProfile>> GetAllPsychologistProfilesAsync(CancellationToken cancellationToken);
    
    Task<IPsychologistProfile?> GetPsychologistProfileByIdAsync(int id, CancellationToken cancellationToken);

    Task<IPsychologistProfile> CreatePsychologistProfileAsync(
        ICreatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken);
    
    Task<IPsychologistProfile> UpdatePsychologistProfileAsync(
        int id, 
        IUpdatePsychologistProfile psychologistProfileData, 
        CancellationToken cancellationToken);
    
    Task<IPsychologistProfile> ActivatePsychologistProfileAsync(int id, CancellationToken cancellationToken);
    
    Task<IPsychologistProfile> DeactivatePsychologistProfileAsync(int id, CancellationToken cancellationToken);
}