using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.PsychologistProfile;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class PsychologistProfileServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<PsychologistProfile>> _psychologistProfileRepositoryMock;
    private readonly IPsychologistProfileService _psychologistProfileService;
    private readonly Mock<IRepository<User>> _userRepositoryMock;

    public PsychologistProfileServiceTests(ApplicationFixture applicationFixture)
    {
        _psychologistProfileRepositoryMock = applicationFixture.PsychologistProfileRepositoryMock;
        _psychologistProfileService = applicationFixture.PsychologistProfileService;
        _userRepositoryMock = applicationFixture.UserRepositoryMock;
    }

    #region GetPsychologistProfileByIdAsync
    [Fact]
    public async Task GetPsychologistProfileByIdAsync_ValidData_Success()
    {
        // Arrange
        const int psychologistProfileId = 1;
        var psychologistProfileData = GetIvanovPsychologistProfile();

        _psychologistProfileRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetIvanovPsychologistProfile());
        
        // Act
        var psychologistProfile =
            await _psychologistProfileService.GetPsychologistProfileByIdAsync(psychologistProfileId, default);
        
        // Assert
        psychologistProfile!.Name.Should().Be(psychologistProfileData.Name);
        psychologistProfile.Description.Should().Be(psychologistProfileData.Description);
        psychologistProfile.IsActive.Should().Be(psychologistProfileData.IsActive);
        psychologistProfile.IncludingQueries.Should().Be(psychologistProfileData.IncludingQueries);
        psychologistProfile.ExcludingQueries.Should().Be(psychologistProfileData.ExcludingQueries);
        psychologistProfile.TimeZone.Should().Be(psychologistProfileData.TimeZone);
        psychologistProfile.UserId.Should().Be(psychologistProfileData.UserId);
    }

    [Fact]
    public async Task GetPsychologistProfileByIdAsync_PsychologistProfileIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int psychologistProfileId = 3;
        _psychologistProfileRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PsychologistProfile)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _psychologistProfileService.GetPsychologistProfileByIdAsync(psychologistProfileId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region CreatePsychologistProfileAsync
    [Fact]
    public async Task CreatePsychologistProfileAsync_ValidData_Success()
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();

        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock.Setup(repository => repository.AddAsync(
                It.Is<PsychologistProfile>(psychologistProfile =>
                    psychologistProfile.Name == createPsychologistProfileRequest.Name &&
                    psychologistProfile.UserId == createPsychologistProfileRequest.UserId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var psychologistProfile =
            await _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default);
        
        // Assert
        psychologistProfile.Name.Should().Be(createPsychologistProfileRequest.Name);
        psychologistProfile.Description.Should().Be(createPsychologistProfileRequest.Description);
        psychologistProfile.IncludingQueries.Should().Be(createPsychologistProfileRequest.IncludingQueries);
        psychologistProfile.ExcludingQueries.Should().Be(createPsychologistProfileRequest.ExcludingQueries);
        psychologistProfile.IsActive.Should().Be(true);
        psychologistProfile.UserId.Should().Be(createPsychologistProfileRequest.UserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreatePsychologistProfileAsync_NameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.Name = name;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreatePsychologistProfileAsync_DescriptionIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string description)
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.Description = description;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreatePsychologistProfileAsync_TimeZoneIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string timeZone)
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.TimeZone = timeZone;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreatePsychologistProfileAsync_IncludingQueriesIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string includingQueries)
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.IncludingQueries = includingQueries;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreatePsychologistProfileAsync_ExcludingQueriesIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string excludingQueries)
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.ExcludingQueries = excludingQueries;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePsychologistProfileAsync_UserIsNotFound_ThrowIncorrectDataException()
    {
        // Arrange
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        createPsychologistProfileRequest.UserId = 3;

        _userRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePsychologistProfileAsync_UserRoleIsAdmin_ThrowIncorrectDataException()
    {
        // Arrange
        var user = GetPetrovUser();
        user.RoleId = (int)RoleType.Admin;
        
        var createPsychologistProfileRequest = GetCreatePsychologistProfileRequest();
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(createPsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.CreatePsychologistProfileAsync(createPsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region UpdatePsychologistProfileAsync
    [Fact]
    public async Task UpdatePsychologistProfileAsync_ValidData_Success()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        var psychologistProfileData = GetPetrovPsychologistProfile();

        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(psychologistProfileData);
        _psychologistProfileRepositoryMock.Setup(repository => repository.UpdateAsync(
                It.Is<PsychologistProfile>(psychologistProfile =>
                    psychologistProfile.Id == psychologistProfileId &&
                    psychologistProfile.Name == updatePsychologistProfileRequest.Name &&
                    psychologistProfile.UserId == updatePsychologistProfileRequest.UserId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PsychologistProfile()
            {
                Id = psychologistProfileId,
                Name = updatePsychologistProfileRequest.Name,
                Description = updatePsychologistProfileRequest.Description,
                IncludingQueries = updatePsychologistProfileRequest.IncludingQueries,
                ExcludingQueries = updatePsychologistProfileRequest.ExcludingQueries,
                TimeZone = updatePsychologistProfileRequest.TimeZone,
                UserId = updatePsychologistProfileRequest.UserId,
                IsActive = psychologistProfileData.IsActive
            });
        
        // Act
        var psychologistProfile =
            await _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default);
        
        // Assert
        psychologistProfile.Id.Should().Be(psychologistProfileId);
        psychologistProfile.Name.Should().Be(updatePsychologistProfileRequest.Name);
        psychologistProfile.Description.Should().Be(updatePsychologistProfileRequest.Description);
        psychologistProfile.IncludingQueries.Should().Be(updatePsychologistProfileRequest.IncludingQueries);
        psychologistProfile.ExcludingQueries.Should().Be(updatePsychologistProfileRequest.ExcludingQueries);
        psychologistProfile.IsActive.Should().Be(psychologistProfileData.IsActive);
        psychologistProfile.UserId.Should().Be(updatePsychologistProfileRequest.UserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdatePsychologistProfileAsync_NameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.Name = name;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdatePsychologistProfileAsync_DescriptionIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string description)
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.Description = description;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdatePsychologistProfileAsync_TimeZoneIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string timeZone)
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.TimeZone = timeZone;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdatePsychologistProfileAsync_IncludingQueriesIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string includingQueries)
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.IncludingQueries = includingQueries;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdatePsychologistProfileAsync_ExcludingQueriesIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string excludingQueries)
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.ExcludingQueries = excludingQueries;
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Fact]
    public async Task UpdatePsychologistProfileAsync_PsychologistProfileIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int psychologistProfileId = 3;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();

        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PsychologistProfile)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Fact]
    public async Task UpdatePsychologistProfileAsync_UserIsNotFound_ThrowIncorrectDataException()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        updatePsychologistProfileRequest.UserId = 3;

        _userRepositoryMock.Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdatePsychologistProfileAsync_UserRoleIsAdmin_ThrowIncorrectDataException()
    {
        // Arrange
        var user = GetPetrovUser();
        user.RoleId = (int)RoleType.Admin;
        
        const int psychologistProfileId = 2;
        var updatePsychologistProfileRequest = GetUpdatePsychologistProfileRequest();
        
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(updatePsychologistProfileRequest.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovPsychologistProfile());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _psychologistProfileService.UpdatePsychologistProfileAsync(psychologistProfileId,
                updatePsychologistProfileRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region ActivatePsychologistProfileAsync
    [Fact]
    public async Task ActivatePsychologistProfileAsync_ValidData_Success()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var inactivePsychologistProfile = GetPetrovPsychologistProfile();
        inactivePsychologistProfile.IsActive = false;

        var userData = GetPetrovUser();
        
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactivePsychologistProfile);
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(inactivePsychologistProfile.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userData);
        
        // Act
        await _psychologistProfileService.ActivatePsychologistProfileAsync(psychologistProfileId, default);
        
        // Assert
        inactivePsychologistProfile.IsActive.Should().Be(true);
    }
    
    [Fact]
    public async Task ActivatePsychologistProfileAsync_PsychologistProfileIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int psychologistProfileId = 3;

        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PsychologistProfile)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _psychologistProfileService.ActivatePsychologistProfileAsync(psychologistProfileId, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task ActivatePsychologistProfileAsync_UserIsNotFound_ThrowInternalPlatformErrorException()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var inactivePsychologistProfile = GetPetrovPsychologistProfile();
        inactivePsychologistProfile.IsActive = false;
        
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactivePsychologistProfile);
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(inactivePsychologistProfile.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<InternalPlatformErrorException>(() =>
            _psychologistProfileService.ActivatePsychologistProfileAsync(psychologistProfileId, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task ActivatePsychologistProfileAsync_UserIsBlocked_ThrowBusinessLogicException()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var inactivePsychologistProfile = GetPetrovPsychologistProfile();
        inactivePsychologistProfile.IsActive = false;

        var userData = GetPetrovUser();
        userData.IsBlocked = true;
        
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactivePsychologistProfile);
        _userRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(inactivePsychologistProfile.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userData);
        
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(() =>
            _psychologistProfileService.ActivatePsychologistProfileAsync(psychologistProfileId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region DeactivatePsychologistProfileAsync
    [Fact]
    public async Task DeactivatePsychologistProfileAsync_ValidData_Success()
    {
        // Arrange
        const int psychologistProfileId = 2;
        var activePsychologistProfile = GetPetrovPsychologistProfile();
        
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activePsychologistProfile);
        
        // Act
        await _psychologistProfileService.DeactivatePsychologistProfileAsync(psychologistProfileId, default);
        
        // Assert
        activePsychologistProfile.IsActive.Should().Be(false);
    }
    
    [Fact]
    public async Task DeactivatePsychologistProfileAsync_PsychologistProfileIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int psychologistProfileId = 3;

        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetByIdAsync(psychologistProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PsychologistProfile)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _psychologistProfileService.DeactivatePsychologistProfileAsync(psychologistProfileId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    private static PsychologistProfile GetIvanovPsychologistProfile() => new()
    {
        Id = 1,
        Name = "Ivan Ivanov",
        Description = "No Description",
        IncludingQueries = "List 1",
        ExcludingQueries = "List 2",
        IsActive = true,
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        UserId = 1
    };

    private static User GetPetrovUser() => new()
    {
        Id = 2,
        Name = "Petr Petrov",
        Email = "petrov@fakemail.com",
        Password = "qwerty",
        IsBlocked = false,
        RoleId = (int)RoleType.Psychologist
    };

    private static PsychologistProfile GetPetrovPsychologistProfile() => new()
    {
        Id = 2,
        Name = "Petr Petrov",
        Description = "Certified psychologist",
        IncludingQueries = "List 1",
        ExcludingQueries = "List 2",
        IsActive = true,
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        UserId = 2
    };

    private static CreatePsychologistProfileRequest GetCreatePsychologistProfileRequest() => new()
    {
        Name = "Petr Petrov",
        Description = "Certified psychologist",
        IncludingQueries = "List 1",
        ExcludingQueries = "List 2",
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        UserId = 2
    };

    private static UpdatePsychologistProfileRequest GetUpdatePsychologistProfileRequest() => new()
    {
        Name = "Petr Petrov",
        Description = "Certified psychologist, hypnosis practitioner",
        IncludingQueries = "List 1",
        ExcludingQueries = "List 2",
        TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
        UserId = 2
    };
}