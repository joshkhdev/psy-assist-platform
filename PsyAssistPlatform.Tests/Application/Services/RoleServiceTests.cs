using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class RoleServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Role>> _roleRepositoryMock;
    private readonly IRoleService _roleService;

    public RoleServiceTests(ApplicationFixture applicationFixture)
    {
        _roleRepositoryMock = applicationFixture.RoleRepositoryMock;
        _roleService = applicationFixture.RoleService;
    }
    
    [Fact]
    public async Task GetRoleByIdAsync_ValidData_Success()
    {
        // Arrange
        const int roleId = 3;
        var roleData = GetPsychologistRole();

        _roleRepositoryMock.Setup(repository => repository.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(roleData);
        
        // Act
        var role = await _roleService.GetRoleByIdAsync(roleId, default);
        
        // Assert
        role!.Name.Should().Be(roleData.Name);
    }

    [Fact]
    public async Task GetRoleByIdAsync_RoleIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int roleId = 4;
        _roleRepositoryMock.Setup(repository => repository.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() => _roleService.GetRoleByIdAsync(roleId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }

    private static Role GetPsychologistRole() => new()
    {
        Id = 3,
        Name = "Psychologist"
    };
}