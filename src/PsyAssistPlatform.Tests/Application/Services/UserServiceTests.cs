using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.User;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class UserServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly IUserService _userService;
    private readonly Mock<IRepository<Role>> _roleRepositoryMock;
    private readonly Mock<IRepository<PsychologistProfile>> _psychologistProfileRepositoryMock;

    public UserServiceTests(ApplicationFixture applicationFixture)
    {
        _userRepositoryMock = applicationFixture.UserRepositoryMock;
        _userService = applicationFixture.UserService;
        _roleRepositoryMock = applicationFixture.RoleRepositoryMock;
        _psychologistProfileRepositoryMock = applicationFixture.PsychologistProfileRepositoryMock;
    }
    
    #region GetUserByIdAsync
    [Fact]
    public async Task GetUserByIdAsync_ValidData_Success()
    {
        // Arrange
        const int userId = 1;
        var userData = GetIvanovUser();

        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userData);
        
        // Act
        var user = await _userService.GetUserByIdAsync(userId, default);
        
        // Assert
        user!.Name.Should().Be(userData.Name);
        user.Email.Should().Be(userData.Email);
        user.RoleId.Should().Be(userData.RoleId);
        user.IsBlocked.Should().Be(userData.IsBlocked);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int userId = 3;
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetUserByIdAsync(userId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region CreateUserAsync
    [Fact]
    public async Task CreateUserAsync_ValidData_Success()
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();

        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        _userRepositoryMock
            .Setup(repository =>
                repository.AddAsync(
                    It.Is<User>(user => user.Name == createUserRequest.Name && user.Email == createUserRequest.Email),
                    It.IsAny<CancellationToken>())).ReturnsAsync(GetPetrovUser());
        
        // Act
        var user = await _userService.CreateUserAsync(createUserRequest, default);
        
        // Assert
        user.Name.Should().Be(createUserRequest.Name);
        user.Email.Should().Be(createUserRequest.Email);
        user.Password.Should().Be(createUserRequest.Password);
        user.IsBlocked.Should().Be(false);
        user.RoleId.Should().Be(createUserRequest.RoleId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateUserAsync_NameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();
        createUserRequest.Name = name;
        
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.CreateUserAsync(createUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateUserAsync_EmailIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string email)
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();
        createUserRequest.Email = email;
        
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.CreateUserAsync(createUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateUserAsync_PasswordIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string password)
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();
        createUserRequest.Password = password;
        
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.CreateUserAsync(createUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("petrov.com")]
    [InlineData("petrov@com")]
    [InlineData("@petrov.com")]
    [InlineData("@com")]
    public async Task CreateUserAsync_IncorrectEmailAddressFormat_ThrowIncorrectDataException(string email)
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();
        createUserRequest.Email = email;
        
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.CreateUserAsync(createUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateUserAsync_RoleIsNotFound_ThrowIncorrectDataException()
    {
        // Arrange
        var createUserRequest = GetCreateUserRequest();
        createUserRequest.RoleId = 4;
        
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(createUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.CreateUserAsync(createUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region UpdateUserAsync
    [Fact]
    public async Task UpdateUserAsync_ValidData_Success()
    {
        // Arrange
        const int userId = 2;
        var userData = GetPetrovUser();
        var updateUserRequest = GetUpdateUserRequest();

        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userData);
        _userRepositoryMock.Setup(repository => repository.UpdateAsync(It.Is<User>(user =>
                    user.Id == userId && user.Name == updateUserRequest.Name && user.Email == updateUserRequest.Email),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User()
            {
                Id = userId,
                Name = updateUserRequest.Name,
                Email = updateUserRequest.Email,
                Password = updateUserRequest.Password,
                IsBlocked = userData.IsBlocked,
                RoleId = updateUserRequest.RoleId
            });
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var user = await _userService.UpdateUserAsync(userId, updateUserRequest, default);
        
        // Assert
        user.Id.Should().Be(userId);
        user.Name.Should().Be(updateUserRequest.Name);
        user.Email.Should().Be(updateUserRequest.Email);
        user.Password.Should().Be(updateUserRequest.Password);
        user.RoleId.Should().Be(updateUserRequest.RoleId);
        user.IsBlocked.Should().Be(userData.IsBlocked);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdateUserAsync_NameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        const int userId = 2;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.Name = name;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdateUserAsync_EmailIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string email)
    {
        // Arrange
        const int userId = 2;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.Email = email;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdateUserAsync_PasswordIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string password)
    {
        // Arrange
        const int userId = 2;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.Password = password;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("petrov.com")]
    [InlineData("petrov@com")]
    [InlineData("@petrov.com")]
    [InlineData("@com")]
    public async Task UpdateUserAsync_IncorrectEmailAddressFormat_ThrowIncorrectDataException(string email)
    {
        // Arrange
        const int userId = 2;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.Email = email;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUserAsync_UserIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int userId = 3;
        var updateUserRequest = GetUpdateUserRequest();
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    
    [Fact]
    public async Task UpdateUserAsync_RoleIsNotFound_ThrowIncorrectDataException()
    {
        // Arrange
        const int userId = 2;
        const int roleId = 4;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.RoleId = roleId;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUserAsync_UserCannotBeAdmin_ThrowIncorrectDataException()
    {
        // Arrange
        const int userId = 2;
        var updateUserRequest = GetUpdateUserRequest();
        updateUserRequest.RoleId = (int)RoleType.Admin;

        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPetrovUser());
        _roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(updateUserRequest.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPsychologistRole());
        _psychologistProfileRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<PsychologistProfile, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetActivePsychologistProfiles());
        
        // Act
        var exception =
            await Assert.ThrowsAsync<IncorrectDataException>(() =>
                _userService.UpdateUserAsync(userId, updateUserRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region UnblockUserAsync
    [Fact]
    public async Task UnblockUserAsync_ValidData_Success()
    {
        // Arrange
        const int userId = 2;
        var user = GetPetrovUser();
        user.IsBlocked = true;
        
        var psychologistProfiles = GetInactivePsychologistProfiles();
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<PsychologistProfile, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(psychologistProfiles);
        
        // Act
        await _userService.UnblockUserAsync(userId, default);
        
        // Assert
        user.IsBlocked.Should().Be(false);
        psychologistProfiles.Single().IsActive.Should().Be(true);
    }
    
    [Fact]
    public async Task UnblockUserAsync_UserIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int userId = 3;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _userService.UnblockUserAsync(userId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    #region BlockUserAsync
    [Fact]
    public async Task BlockUserAsync_ValidData_Success()
    {
        // Arrange
        const int userId = 2;
        var user = GetPetrovUser();
        var psychologistProfiles = GetActivePsychologistProfiles();
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _psychologistProfileRepositoryMock
            .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<PsychologistProfile, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(psychologistProfiles);
        
        // Act
        await _userService.BlockUserAsync(userId, default);
        
        // Assert
        user.IsBlocked.Should().Be(true);
        psychologistProfiles.Single().IsActive.Should().Be(false);
    }

    [Fact]
    public async Task BlockUserAsync_UserIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int userId = 3;
        
        _userRepositoryMock.Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _userService.BlockUserAsync(userId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    private static CreateUserRequest GetCreateUserRequest() => new()
    {
        Name = "Petr Petrov",
        Email = "petrov@fakemail.com",
        Password = "qwerty",
        RoleId = 3
    };

    private static UpdateUserRequest GetUpdateUserRequest() => new()
    {
        Name = "Peter Petrov",
        Email = "petrov@mail.com",
        Password = "qwerty",
        RoleId = 3
    };
    
    private static User GetIvanovUser() => new()
    {
        Id = 1,
        Name = "Ivan Ivanov",
        Email = "ivanov@goooooooogle.org",
        IsBlocked = false,
        Password = "qwerty",
        RoleId = 3
    };

    private static User GetPetrovUser() => new()
    {
        Id = 2,
        Name = "Petr Petrov",
        Email = "petrov@fakemail.com",
        Password = "qwerty",
        IsBlocked = false,
        RoleId = 3
    };
    
    private static Role GetPsychologistRole() => new()
    {
        Id = 3,
        Name = "Psychologist"
    };

    private static List<PsychologistProfile> GetActivePsychologistProfiles() =>
    [
        new PsychologistProfile()
        {
            Id = 2,
            Name = "Petr Petrov",
            Description = "Certified psychologist",
            IncludingQueries = "List 1",
            ExcludingQueries = "List 2",
            IsActive = true,
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 2
        }
    ];
    
    private static List<PsychologistProfile> GetInactivePsychologistProfiles() =>
    [
        new PsychologistProfile()
        {
            Id = 2,
            Name = "Petr Petrov",
            Description = "Certified psychologist",
            IncludingQueries = "List 1",
            ExcludingQueries = "List 2",
            IsActive = false,
            TimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString(),
            UserId = 2
        }
    ];
}