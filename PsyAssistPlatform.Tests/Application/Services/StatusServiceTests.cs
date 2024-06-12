using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class StatusServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Status>> _statusRepositoryMock;
    private readonly IStatusService _statusService;

    public StatusServiceTests(ApplicationFixture applicationFixture)
    {
        _statusRepositoryMock = applicationFixture.StatusRepositoryMock;
        _statusService = applicationFixture.StatusService;
    }

    [Fact]
    public async Task GetStatusByIdAsync_ValidData_Success()
    {
        // Arrange
        const int statusId = 3;
        var statusData = GetStatus();

        _statusRepositoryMock.Setup(repository => repository.GetByIdAsync(statusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusData);
        
        // Act
        var status = await _statusService.GetStatusByIdAsync(statusId, default);
        
        // Assert
        status!.Name.Should().Be(statusData.Name);
    }

    [Fact]
    public async Task GetStatusByIdAsync_StatusIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int statusId = 4;
        _statusRepositoryMock.Setup(repository => repository.GetByIdAsync(statusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Status)null!);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() => _statusService.GetStatusByIdAsync(statusId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }

    private static Status GetStatus() => new()
    {
        Id = 3,
        Name = "Completed"
    };
}