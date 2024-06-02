using FluentValidation;
using PsyAssistPlatform.Application;

namespace PsyAssistPlatform.WebApi.Models.Contact;

public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
{
    private const string AllContactDetailsCannotBeMessage = "All contact details (email, phone, telegram) cannot be empty";
    private const string IncorrectEmailAddressFormatMessage = "Incorrect email address format";
    private const string IncorrectPhoneNumberFormatMessage = "Incorrect phone number format";
    
    public UpdateContactRequestValidator()
    {
        RuleFor(request => request.Email)
            .EmailAddress()
            .WithMessage(IncorrectEmailAddressFormatMessage)
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage(AllContactDetailsCannotBeMessage)
            .EmailAddress()
            .WithMessage(IncorrectEmailAddressFormatMessage)
            .When(request => string.IsNullOrWhiteSpace(request.Phone) && string.IsNullOrWhiteSpace(request.Telegram));
        
        RuleFor(request => request.Phone)
            .Must(Validator.PhoneNumberValidator!)
            .WithMessage(IncorrectPhoneNumberFormatMessage)
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage(AllContactDetailsCannotBeMessage)
            .Must(Validator.PhoneNumberValidator!)
            .WithMessage(IncorrectPhoneNumberFormatMessage)
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Telegram));
        
        RuleFor(request => request.Telegram)
            .NotNull()
            .NotEmpty()
            .WithMessage(AllContactDetailsCannotBeMessage)
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone));
    }
}