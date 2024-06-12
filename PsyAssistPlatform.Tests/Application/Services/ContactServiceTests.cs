using FluentAssertions;
using Moq;
using PsyAssistPlatform.Application.Exceptions;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.WebApi.Models.Contact;
using Xunit;

namespace PsyAssistPlatform.Tests.Application.Services;

public class ContactServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock;
    private readonly IContactService _contactService;

    public ContactServiceTests(ApplicationFixture applicationFixture)
    {
        _contactRepositoryMock = applicationFixture.ContactRepositoryMock;
        _contactService = applicationFixture.ContactService;
    }

    #region GetContactByIdAsync
    [Fact]
    public async Task GetContactByIdAsync_ValidData_Success()
    {
        // Arrange
        const int contactId = 1;
        var contactData = GetJohnDoeContact();
        
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactData);
        
        // Act
        var contact = await _contactService.GetContactByIdAsync(contactId, default);
        
        // Assert
        contact!.Email.Should().Be(contactData.Email);
        contact.Phone.Should().Be(contactData.Phone);
        contact.Telegram.Should().Be(contactData.Telegram);
    }

    [Fact]
    public async Task GetContactByIdAsync_ContactIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int contactId = 2;
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contact)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _contactService.GetContactByIdAsync(contactId, default));
        
        // Assert
        exception.Should().NotBeNull();
    }
    #endregion

    #region UpdateContactAsync
    [Fact]
    public async Task UpdateContactAsync_ValidData_Success()
    {
        // Arrange
        const int contactId = 1;
        var updateContactRequest = GetValidUpdateContactRequest();

        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetJohnDoeContact());
        _contactRepositoryMock.Setup(repository =>
                repository.UpdateAsync(It.Is<Contact>(contact => contact.Id == contactId),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Contact()
            {
                Id = contactId,
                Email = updateContactRequest.Email,
                Phone = updateContactRequest.Phone,
                Telegram = updateContactRequest.Telegram
            });
        
        // Act
        var updatedContact =
            await _contactService.UpdateContactAsync(contactId, GetValidUpdateContactRequest(), default);
        
        // Assert
        updatedContact.Id.Should().Be(contactId);
        updatedContact.Email.Should().Be(updateContactRequest.Email);
        updatedContact.Phone.Should().Be(updateContactRequest.Phone);
        updatedContact.Telegram.Should().Be(updateContactRequest.Telegram);
    }

    [Fact]
    public async Task UpdateContactAsync_ContactIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int contactId = 2;
        var updateContactRequest = GetValidUpdateContactRequest();
        
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contact)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _contactService.UpdateContactAsync(contactId, updateContactRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData(" ", " ", " ")]
    public async Task UpdateContactAsync_AllContactDetailsAreEmptyOrWhiteSpace_ThrowIncorrectDataException(
        string email,
        string phone, 
        string telegram)
    {
        // Arrange
        const int contactId = 1;
        var updateContactRequest = new UpdateContactRequest()
        {
            Email = email,
            Phone = phone,
            Telegram = telegram
        };
        
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetJohnDoeContact());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _contactService.UpdateContactAsync(contactId, updateContactRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("johndoe.com")]
    [InlineData("johndoe@com")]
    [InlineData("@johndoe.com")]
    [InlineData("@com")]
    public async Task UpdateContactAsync_IncorrectEmailAddressFormat_ThrowIncorrectDataException(string email)
    {
        // Arrange
        const int contactId = 1;
        var updateContactRequest = new UpdateContactRequest()
        {
            Email = email,
            Phone = "",
            Telegram = ""
        };
        
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetJohnDoeContact());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _contactService.UpdateContactAsync(contactId, updateContactRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [InlineData("79991234567")]
    [InlineData("+7999123")]
    [InlineData("+7(999)123-45-67")]
    [InlineData("+7-(999)-123-45-67")]
    public async Task UpdateContactAsync_IncorrectPhoneNumberFormat_ThrowIncorrectDataException(string phone)
    {
        // Arrange
        const int contactId = 1;
        var updateContactRequest = new UpdateContactRequest()
        {
            Email = "",
            Phone = phone,
            Telegram = ""
        };
        
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetJohnDoeContact());
        
        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _contactService.UpdateContactAsync(contactId, updateContactRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }
    #endregion
    
    private static Contact GetJohnDoeContact() => new()
    {
        Id = 1,
        Telegram = "@johndoe_fake",
        Email = "john.doe@fakemail.com",
        Phone = "+79991234567"
    };

    private static UpdateContactRequest GetValidUpdateContactRequest() => new()
    {
        Telegram = "@doe_fake",
        Email = "doe@fakemail.com",
        Phone = "+71111234567"
    };
}