using FluentValidation;

namespace PsyAssistPlatform.WebApi.Models.Contact;

public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
{
    public UpdateContactRequestValidator()
    {
        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .EmailAddress()
            .WithMessage("Incorrect email address format")
            .When(request => string.IsNullOrWhiteSpace(request.Phone) && string.IsNullOrWhiteSpace(request.Telegram));
        RuleFor(request => request.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Telegram));
        RuleFor(request => request.Telegram)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone));
    }
}