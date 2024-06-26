using AutoMapper;
using Moq;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Application.Mapping;
using PsyAssistPlatform.Application.Services;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Tests.Application;

public class ApplicationFixture : IDisposable
{
    public ApplicationFixture()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

        ApproachRepositoryMock = new Mock<IRepository<Approach>>();
        ApproachService = new ApproachService(ApproachRepositoryMock.Object, mapper);
        ContactRepositoryMock = new Mock<IRepository<Contact>>();
        ContactService = new ContactService(ContactRepositoryMock.Object, mapper);
        QuestionnaireRepositoryMock = new Mock<IRepository<Questionnaire>>();
        QuestionnaireService =
            new QuestionnaireService(QuestionnaireRepositoryMock.Object, ContactRepositoryMock.Object, mapper);
        RoleRepositoryMock = new Mock<IRepository<Role>>();
        RoleService = new RoleService(RoleRepositoryMock.Object, mapper);
        UserRepositoryMock = new Mock<IRepository<User>>();
        PsychologistProfileRepositoryMock = new Mock<IRepository<PsychologistProfile>>();
        UserService = new UserService(UserRepositoryMock.Object, PsychologistProfileRepositoryMock.Object,
            RoleRepositoryMock.Object, mapper);
        PsychologistProfileService = new PsychologistProfileService(PsychologistProfileRepositoryMock.Object,
            UserRepositoryMock.Object, mapper);
        StatusRepositoryMock = new Mock<IRepository<Status>>();
        StatusService = new StatusService(StatusRepositoryMock.Object, mapper);
    }

    public Mock<IRepository<Approach>> ApproachRepositoryMock { get; init; }

    public IApproachService ApproachService { get; init; }
    
    public Mock<IRepository<Contact>> ContactRepositoryMock { get; init; }
    
    public IContactService ContactService { get; init; }
    
    public Mock<IRepository<PsychologistProfile>> PsychologistProfileRepositoryMock { get; init; }
    
    public IPsychologistProfileService PsychologistProfileService { get; init; }
    
    public Mock<IRepository<Questionnaire>> QuestionnaireRepositoryMock { get; init; }
    
    public IQuestionnaireService QuestionnaireService { get; init; }
    
    public Mock<IRepository<User>> UserRepositoryMock { get; init; }
    
    public IUserService UserService { get; init; }
    
    public Mock<IRepository<Role>> RoleRepositoryMock { get; init; }
    
    public IRoleService RoleService { get; init; }
    
    public Mock<IRepository<Status>> StatusRepositoryMock { get; init; }
    
    public IStatusService StatusService { get; init; }

    public void Dispose()
    {
    }
}