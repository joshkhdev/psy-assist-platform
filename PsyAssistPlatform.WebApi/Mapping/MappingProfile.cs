using AutoMapper;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Psychologist;

namespace PsyAssistPlatform.WebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Psychologist, PsychologistResponse>();
            CreateMap<Psychologist, PsychologistShortResponse>();
            CreateMap<CreatePsychologistRequest, Psychologist>();
            CreateMap<UpdatePsychologistRequest, Psychologist>();
        }
    }
}
