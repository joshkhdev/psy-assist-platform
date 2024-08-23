using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Questionnaire;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class QuestionnaireServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Questionnaire>> _questionnaireRepositoryMock;
    private readonly IQuestionnaireService _questionnaireService;
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock;
    private readonly Mock<IRepository<PsyRequest>> _psyRequestRepositoryMock;

    public QuestionnaireServiceTests(ApplicationFixture applicationFixture)
    {
        _questionnaireRepositoryMock = applicationFixture.QuestionnaireRepositoryMock;
        _questionnaireService = applicationFixture.QuestionnaireService;
        _contactRepositoryMock = applicationFixture.ContactRepositoryMock;
        _psyRequestRepositoryMock = applicationFixture.PsyRequestRepositoryMock;
    }
    
    #region GetQuestionnaireByIdAsync
    [Fact]
    public async Task GetQuestionnaireByIdAsync_ValidData_Success()
    {
        // Arrange
        const int questionnaireId = 1;
        var questionnaireData = GetJohnDoeQuestionnaire();

        _questionnaireRepositoryMock
            .Setup(repository => repository.GetByIdAsync(questionnaireId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questionnaireData);
        
        // Act
        var questionnaire = await _questionnaireService.GetQuestionnaireByIdAsync(questionnaireId, default);
        
        // Assert
        questionnaire!.Name.Should().Be(questionnaireData.Name);
        questionnaire.Pronouns.Should().Be(questionnaireData.Pronouns);
        questionnaire.Age.Should().Be(questionnaireData.Age);
        questionnaire.TimeZone.Should().Be(questionnaireData.TimeZone);
        questionnaire.Telegram.Should().Be(questionnaireData.Contact.Telegram);
        questionnaire.Email.Should().Be(questionnaireData.Contact.Email);
        questionnaire.Phone.Should().Be(questionnaireData.Contact.Phone);
        questionnaire.NeuroDifferences.Should().Be(questionnaireData.NeuroDifferences);
        questionnaire.MentalSpecifics.Should().Be(questionnaireData.MentalSpecifics);
        questionnaire.PsyWishes.Should().Be(questionnaireData.PsyWishes);
        questionnaire.PsyQuery.Should().Be(questionnaireData.PsyQuery);
        questionnaire.TherapyExperience.Should().Be(questionnaireData.TherapyExperience);
        questionnaire.IsForPay.Should().Be(questionnaireData.IsForPay);
    }

    [Fact]
    public async Task GetQuestionnaireByIdAsync_QuestionnaireIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int questionnaireId = 2;
        _questionnaireRepositoryMock
            .Setup(repository => repository.GetByIdAsync(questionnaireId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Questionnaire)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _questionnaireService.GetQuestionnaireByIdAsync(questionnaireId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region CreateQuestionnaireAsync
    [Fact]
    public async Task CreateQuestionnaireAsync_ValidData_Success()
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();

        _contactRepositoryMock.Setup(repository => repository.AddAsync(
                It.Is<Contact>(contact =>
                    contact.Email == createQuestionnaireRequest.Email &&
                    contact.Phone == createQuestionnaireRequest.Phone &&
                    contact.Telegram == createQuestionnaireRequest.Telegram), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetJaneDoeContact());
        _questionnaireRepositoryMock.Setup(repository =>
            repository.AddAsync(
                It.Is<Questionnaire>(questionnaire => questionnaire.Name == createQuestionnaireRequest.Name &&
                                                      questionnaire.ContactId == GetJaneDoeContact().Id),
                It.IsAny<CancellationToken>())).ReturnsAsync(GetJaneDoeQuestionnaire());
        _psyRequestRepositoryMock
            .Setup(repository =>
                repository.AddAsync(
                    It.Is<PsyRequest>(psyRequest => psyRequest.QuestionnaireId == GetJaneDoeContact().Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(GetJaneDoePsyRequest());
        
        // Act
        var questionnaire = await _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default);

        // Assert
        questionnaire.Name.Should().Be(createQuestionnaireRequest.Name);
        questionnaire.Pronouns.Should().Be(createQuestionnaireRequest.Pronouns);
        questionnaire.Age.Should().Be(createQuestionnaireRequest.Age);
        questionnaire.TimeZone.Should().Be(createQuestionnaireRequest.TimeZone);
        questionnaire.Telegram.Should().Be(createQuestionnaireRequest.Telegram);
        questionnaire.Email.Should().Be(createQuestionnaireRequest.Email);
        questionnaire.Phone.Should().Be(createQuestionnaireRequest.Phone);
        questionnaire.NeuroDifferences.Should().Be(createQuestionnaireRequest.NeuroDifferences);
        questionnaire.MentalSpecifics.Should().Be(createQuestionnaireRequest.MentalSpecifics);
        questionnaire.PsyWishes.Should().Be(createQuestionnaireRequest.PsyWishes);
        questionnaire.PsyQuery.Should().Be(createQuestionnaireRequest.PsyQuery);
        questionnaire.TherapyExperience.Should().Be(createQuestionnaireRequest.TherapyExperience);
        questionnaire.IsForPay.Should().Be(createQuestionnaireRequest.IsForPay);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData(" ", " ", " ")]
    public async Task CreateQuestionnaireAsync_AllContactDetailsAreEmptyOrWhiteSpace_ThrowIncorrectDataException(
        string email,
        string phone, 
        string telegram)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Email = email;
        createQuestionnaireRequest.Phone = phone;
        createQuestionnaireRequest.Telegram = telegram;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_NameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Name = name;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_PronounsIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string pronouns)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Pronouns = pronouns;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_TimeZoneIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string timeZone)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.TimeZone = timeZone;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_NeuroDifferencesIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string neuroDifferences)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.NeuroDifferences = neuroDifferences;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_PsyQueryIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string psyQuery)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.PsyQuery = psyQuery;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateQuestionnaireAsync_TherapyExperienceIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string therapyExperience)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.TherapyExperience = therapyExperience;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("janedoe.com")]
    [InlineData("janedoe@com")]
    [InlineData("@janedoe.com")]
    [InlineData("@com")]
    public async Task CreateQuestionnaireAsync_IncorrectEmailAddressFormat_ThrowIncorrectDataException(string email)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Email = email;
        createQuestionnaireRequest.Phone = "";
        createQuestionnaireRequest.Telegram = "";
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("79991234567")]
    [InlineData("+7999123")]
    [InlineData("+7(999)123-45-67")]
    [InlineData("+7-(999)-123-45-67")]
    public async Task CreateQuestionnaireAsync_IncorrectPhoneNumberFormat_ThrowIncorrectDataException(string phone)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Email = "";
        createQuestionnaireRequest.Phone = phone;
        createQuestionnaireRequest.Telegram = "";
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    public async Task CreateQuestionnaireAsync_IncorrectAgeValue_ThrowIncorrectDataException(int age)
    {
        // Arrange
        var createQuestionnaireRequest = GetCreateQuestionnaireRequest();
        createQuestionnaireRequest.Age = age;
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _questionnaireService.CreateQuestionnaireAsync(createQuestionnaireRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    private static Questionnaire GetJohnDoeQuestionnaire() => new()
    {
        Id = 1,
        Name = "John Doe",
        Pronouns = "He/Him",
        Age = 25,
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        ContactId = 1,
        Contact = GetJohnDoeContact(),
        NeuroDifferences = "No",
        MentalSpecifics = "No",
        PsyWishes = "No",
        PsyQuery = "I want my therapist to be a woman",
        TherapyExperience = "7 months",
        IsForPay = false,
        RegistrationDate = DateTime.UtcNow
    };
    
    private static Questionnaire GetJaneDoeQuestionnaire() => new()
    {
        Id = 2,
        Name = "Jane Doe",
        Pronouns = "She/Her",
        Age = 30,
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        ContactId = 2,
        Contact = GetJaneDoeContact(),
        NeuroDifferences = "No",
        MentalSpecifics = "No",
        PsyWishes = "No",
        PsyQuery = "No",
        TherapyExperience = "One year",
        IsForPay = true
    };

    private static CreateQuestionnaireRequest GetCreateQuestionnaireRequest() => new()
    {
        Name = "Jane Doe",
        Pronouns = "She/Her",
        Age = 30,
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        Telegram = "@janedoe_fake",
        Email = "jane.doe@fakemail.com",
        Phone = "+79991234568",
        NeuroDifferences = "No",
        MentalSpecifics = "No",
        PsyWishes = "No",
        PsyQuery = "No",
        TherapyExperience = "One year",
        IsForPay = true
    };
    
    private static Contact GetJohnDoeContact() => new()
    {
        Id = 1,
        Telegram = "@johndoe_fake",
        Email = "john.doe@fakemail.com",
        Phone = "+79991234567"
    };
    
    private static Contact GetJaneDoeContact() => new()
    {
        Id = 2,
        Telegram = "@janedoe_fake",
        Email = "jane.doe@fakemail.com",
        Phone = "+79991234568"
    };

    private static PsyRequest GetJaneDoePsyRequest() => new()
    {
        Id = GetJaneDoeQuestionnaire().Id,
        Questionnaire = GetJaneDoeQuestionnaire(),
        PsychologistProfileId = null
    };
}