using AutoMapper;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Psychologist;
using PsyAssistPlatform.WebApi.Models.Role;
using PsyAssistPlatform.WebApi.Models.User;

namespace PsyAssistPlatform.WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapPsychologist();
        MapUser();
        MapRole();
    }

    private void MapPsychologist()
    {
        CreateMap<Psychologist, PsychologistResponse>();
        CreateMap<Psychologist, PsychologistShortResponse>();
        CreateMap<CreatePsychologistRequest, Psychologist>();
        CreateMap<UpdatePsychologistRequest, Psychologist>();
    }

    private void MapUser()
    {
        CreateMap<User, UserResponse>();
    }

    private void MapRole()
    {
        CreateMap<Role, RoleResponse>();
    }
}