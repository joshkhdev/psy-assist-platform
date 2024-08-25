using AutoMapper;
using PsyAssistPlatform.Application.Dto.Approach;
using PsyAssistPlatform.Application.Dto.Contact;
using PsyAssistPlatform.Application.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Dto.Questionnaire;
using PsyAssistPlatform.Application.Dto.Role;
using PsyAssistPlatform.Application.Dto.Status;
using PsyAssistPlatform.Application.Dto.User;
using PsyAssistPlatform.Application.Interfaces.Dto.Approach;
using PsyAssistPlatform.Application.Interfaces.Dto.Contact;
using PsyAssistPlatform.Application.Interfaces.Dto.PsychologistProfile;
using PsyAssistPlatform.Application.Interfaces.Dto.Questionnaire;
using PsyAssistPlatform.Application.Interfaces.Dto.User;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateApproachMap();
        CreateContactMap();
        CreatePsychologistProfileMap();
        CreateRoleMap();
        CreateStatusMap();
        CreateQuestionnaireMap();
        CreateUserMap();
    }
    
    private void CreateApproachMap()
    {
        CreateMap<Approach, ApproachDto>();
        CreateMap<ICreateApproach, Approach>();
        CreateMap<IUpdateApproach, Approach>();
    }
    
    private void CreateContactMap()
    {
        CreateMap<Contact, ContactDto>();
        CreateMap<IUpdateContact, Contact>();
    }

    private void CreatePsychologistProfileMap()
    {
        CreateMap<PsychologistProfile, PsychologistProfileDto>();
        CreateMap<ICreatePsychologistProfile, PsychologistProfile>();
        CreateMap<IUpdatePsychologistProfile, PsychologistProfile>();
    }
    
    private void CreateQuestionnaireMap()
    {
        CreateMap<Questionnaire, QuestionnaireDto>()
            .ForMember(destination => destination.Telegram, opt => opt.MapFrom(src => src.Contact.Telegram))
            .ForMember(destination => destination.Email, opt => opt.MapFrom(src => src.Contact.Email))
            .ForMember(destination => destination.Phone, opt => opt.MapFrom(src => src.Contact.Phone));
        CreateMap<ICreateQuestionnaire, Questionnaire>();
    }
    
    private void CreateRoleMap()
    {
        CreateMap<Role, RoleDto>();
    }
    
    private void CreateStatusMap()
    {
        CreateMap<Status, StatusDto>();
    }

    private void CreateUserMap()
    {
        CreateMap<User, UserDto>();
        CreateMap<ICreateUser, User>();
        CreateMap<IUpdateUser, User>();
    }
}