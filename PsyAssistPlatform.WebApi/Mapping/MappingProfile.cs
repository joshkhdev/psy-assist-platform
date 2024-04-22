using AutoMapper;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Approach;
using PsyAssistPlatform.WebApi.Models.Contact;
using PsyAssistPlatform.WebApi.Models.Psychologist;
using PsyAssistPlatform.WebApi.Models.Questionnaire;
using PsyAssistPlatform.WebApi.Models.Role;
using PsyAssistPlatform.WebApi.Models.Status;
using PsyAssistPlatform.WebApi.Models.User;

namespace PsyAssistPlatform.WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreatePsychologistMap();
        CreateUserMap();
        CreateRoleMap();
        CreateApproachMap();
        CreateStatusMap();
        CreateContactMap();
        CreateQuestionnaireMap();
    }

    private void CreatePsychologistMap()
    {
        CreateMap<Psychologist, PsychologistResponse>();
        CreateMap<Psychologist, PsychologistShortResponse>();
        CreateMap<CreatePsychologistRequest, Psychologist>();
        CreateMap<UpdatePsychologistRequest, Psychologist>();
    }

    private void CreateUserMap()
    {
        CreateMap<User, UserResponse>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();
    }

    private void CreateRoleMap()
    {
        CreateMap<Role, RoleResponse>();
        CreateMap<CreateRoleRequest, Role>();
        CreateMap<UpdateRoleRequest, Role>();
    }

    private void CreateApproachMap()
    {
        CreateMap<Approach, ApproachResponse>();
        CreateMap<CreateApproachRequest, Approach>();
        CreateMap<UpdateApproachRequest, Approach>();
    }

    private void CreateStatusMap()
    {
        CreateMap<Status, StatusResponse>();
        CreateMap<CreateStatusRequest, Status>();
        CreateMap<UpdateStatusRequest, Status>();
    }

    private void CreateContactMap()
    {
        CreateMap<Contact, ContactResponse>();
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();
        CreateMap<Role, RoleShortResponse>();
        CreateMap<CreateRoleRequest, Role>();
        CreateMap<UpdateRoleRequest, Role>();
    }

    private void CreateApproachMap()
    {
        CreateMap<Approach, ApproachResponse>();
        CreateMap<Approach, ApproachShortResponse>();
        CreateMap<CreateApproachRequest, Approach>();
        CreateMap<UpdateApproachRequest, Approach>();
    }

    private void CreateStatusMap()
    {
        CreateMap<Status, StatusResponse>();
        CreateMap<Status, StatusShortResponse>();
        CreateMap<CreateStatusRequest, Status>();
        CreateMap<UpdateStatusRequest, Status>();
    }

    private void CreateContactMap()
    {
        CreateMap<Contact, ContactResponse>();
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();
    }

    private void CreateQuestionnaireMap()
    {
        CreateMap<Questionnaire, QuestionnaireResponse>();
        CreateMap<Questionnaire, QuestionnaireShortResponse>();
        CreateMap<CreateOrUpdateQuestionnaireRequest, Questionnaire>();
    }
}