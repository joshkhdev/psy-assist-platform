using AutoMapper;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Contact;
using PsyAssistPlatform.WebApi.Models.Psychologist;
using PsyAssistPlatform.WebApi.Models.Role;
using PsyAssistPlatform.WebApi.Models.User;

namespace PsyAssistPlatform.WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreatePsychologistMap();
        CreateUserMap();
        CreateRoleMap();
        CreateContactMap();
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
    }

    private void CreateRoleMap()
    {
        CreateMap<Role, RoleResponse>();
    }

    private void CreateContactMap()
    {
        CreateMap<Contact, ContactResponse>();
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();
    }
}