﻿using AutoMapper;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Approach;
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
        CreateApproachMap();
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

    private void CreateApproachMap()
    {
        CreateMap<Approach, ApproachResponse>();
        CreateMap<Approach, ApproachShortResponse>();
        CreateMap<CreateApproachRequest, Approach>();
        CreateMap<UpdateApproachRequest, Approach>();
    }
}