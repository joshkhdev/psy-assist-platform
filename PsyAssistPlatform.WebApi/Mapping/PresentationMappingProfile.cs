using AutoMapper;
using PsyAssistPlatform.Application.Interfaces.Dto.Approach;
using PsyAssistPlatform.Application.Interfaces.Dto.Contact;
using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;
using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;
using PsyAssistPlatform.Application.Interfaces.Dto.Role;
using PsyAssistPlatform.Application.Interfaces.Dto.Status;
using PsyAssistPlatform.Application.Interfaces.Dto.User;
using PsyAssistPlatform.WebApi.Models.Approach;
using PsyAssistPlatform.WebApi.Models.Contact;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;
using PsyAssistPlatform.WebApi.Models.PsyRequest;
using PsyAssistPlatform.WebApi.Models.Questionnaire;
using PsyAssistPlatform.WebApi.Models.Role;
using PsyAssistPlatform.WebApi.Models.Status;
using PsyAssistPlatform.WebApi.Models.User;

namespace PsyAssistPlatform.WebApi.Mapping;

public class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        CreateApproachMap();
        CreateContactMap();
        CreatePsychologistProfileMap();
        CreateRoleMap();
        CreateStatusMap();
        CreateQuestionnaireMap();
        CreateUserMap();
        CreatePsyRequestMap();
        CreatePsyRequestStatusMap();
    }
    
    private void CreateApproachMap()
    {
        CreateMap<IApproach, ApproachResponse>();
    }

    private void CreateContactMap()
    {
        CreateMap<IContact, ContactResponse>();
    }

    private void CreatePsychologistProfileMap()
    {
        CreateMap<IPsychologistProfile, PsychologistProfileResponse>();
    }

    private void CreateQuestionnaireMap()
    {
        CreateMap<IQuestionnaire, QuestionnaireResponse>();
    }
    
    private void CreateRoleMap()
    {
        CreateMap<IRole, RoleResponse>();
    }
    
    private void CreateStatusMap()
    {
        CreateMap<IStatus, StatusResponse>();
    }

    private void CreateUserMap()
    {
        CreateMap<IUser, UserResponse>();
    }

    private void CreatePsyRequestMap()
    {
        CreateMap<IPsyRequestInfo, PsyRequestInfoResponse>();
    }

    private void CreatePsyRequestStatusMap()
    {
        CreateMap<IPsyRequestStatus, PsyRequestStatusResponse>();
    }
}