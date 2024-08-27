using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Approach;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class ApproachServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Approach>> _approachRepositoryMock;
    private readonly IApproachService _approachService;

    public ApproachServiceTests(ApplicationFixture applicationFixture)
    {
        _approachRepositoryMock = applicationFixture.ApproachRepositoryMock;
        _approachService = applicationFixture.ApproachService;
    }

    #region GetApproachByIdAsync
    [Fact]
    public async Task GetApproachByIdAsync_ValidData_Success()
    {
        // Arrange
        const int approachId = 2;
        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHypnosisApproach());

        // Act
        var approach = await _approachService.GetApproachByIdAsync(approachId, default);

        // Assert
        approach!.Name.Should().Be(GetHypnosisApproach().Name);
    }

    [Fact]
    public async Task GetApproachByIdAsync_ApproachIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int approachId = 1;
        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Approach)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _approachService.GetApproachByIdAsync(approachId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region CreateApproachAsync
    [Fact]
    public async Task CreateApproachAsync_ValidData_Success()
    {
        // Arrange
        var createApproachRequest = new CreateApproachRequest()
        {
            Name = "Hypnosis"
        };

        _approachRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<Approach, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _approachRepositoryMock.Setup(repository =>
                repository.AddAsync(It.Is<Approach>(approach => approach.Name == createApproachRequest.Name),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHypnosisApproach());

        // Act
        var approach = await _approachService.CreateApproachAsync(createApproachRequest, default);

        // Assert
        approach.Name.Should().Be(GetHypnosisApproach().Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateApproachAsync_ApproachNameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        var createApproachRequest = new CreateApproachRequest()
        {
            Name = name
        };

        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _approachService.CreateApproachAsync(createApproachRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateApproachAsync_ExistingApproachData_ThrowIncorrectDataException()
    {
        // Arrange
        var createApproachRequest = new CreateApproachRequest()
        {
            Name = "hypnosis"
        };

        _approachRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<Approach, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GetHypnosisApproach()]);

        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _approachService.CreateApproachAsync(createApproachRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region UpdateApproachAsync
    [Fact]
    public async Task UpdateApproachAsync_ValidData_Success()
    {
        // Arrange
        const int approachId = 2;
        var updateApproachRequest = new UpdateApproachRequest()
        {
            Name = "Gestalt therapy"
        };

        _approachRepositoryMock.Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHypnosisApproach());
        _approachRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<Approach, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _approachRepositoryMock.Setup(repository =>
                repository.UpdateAsync(It.Is<Approach>(approach => approach.Id == approachId),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Approach()
            {
                Id = approachId,
                Name = updateApproachRequest.Name
            });

        // Act
        var updatedApproach = await _approachService.UpdateApproachAsync(approachId, updateApproachRequest, default);

        // Assert
        updatedApproach.Id.Should().Be(approachId);
        updatedApproach.Name.Should().Be(updateApproachRequest.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UpdateApproachAsync_ApproachNameIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string name)
    {
        // Arrange
        const int approachId = 2;
        var updateApproachRequest = new UpdateApproachRequest()
        {
            Name = name
        };

        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _approachService.UpdateApproachAsync(approachId, updateApproachRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateApproachAsync_ApproachIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int approachId = 1;
        var updateApproachRequest = new UpdateApproachRequest()
        {
            Name = "Gestalt therapy"
        };

        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Approach)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _approachService.UpdateApproachAsync(approachId, updateApproachRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateApproachAsync_ApproachWithThisNameAlreadyExists_ThrowIncorrectDataException()
    {
        // Arrange
        const int approachId = 2;
        var updateApproachRequest = new UpdateApproachRequest()
        {
            Name = "hypnosis"
        };

        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHypnosisApproach());

        _approachRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<Approach, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GetHypnosisApproach()]);

        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() => 
            _approachService.UpdateApproachAsync(approachId, updateApproachRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region DeleteApproachAsync
    [Fact]
    public async Task DeleteApproachAsync_ValidData_Success()
    {
        // Arrange
        const int approachId = 2;
        Exception? exception = null;

        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHypnosisApproach());

        // Act
        try
        {
            await _approachService.DeleteApproachAsync(approachId, default);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task DeleteApproachAsync_ApproachIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int approachId = 1;

        _approachRepositoryMock
            .Setup(repository => repository.GetByIdAsync(approachId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Approach)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _approachService.DeleteApproachAsync(approachId, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    private static Approach GetHypnosisApproach() => new()
    {
        Id = 2,
        Name = "Hypnosis"
    };
}