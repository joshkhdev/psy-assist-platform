using FluentValidation;
using PsyAssistPlatform.Application;

namespace PsyAssistPlatform.WebApi.Models.Questionnaire;

public class CreateQuestionnaireRequestValidator : AbstractValidator<CreateQuestionnaireRequest>
{
    public CreateQuestionnaireRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name value cannot be null or empty");
        RuleFor(request => request.Pronouns)
            .NotNull()
            .NotEmpty()
            .WithMessage("Pronouns value cannot be null or empty");
        RuleFor(request => request.Age)
            .GreaterThanOrEqualTo(16)
            .WithMessage("Age value must be at least 16");
        RuleFor(request => request.TimeZone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Time zone value cannot be null or empty");
        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .EmailAddress()
            .WithMessage("Incorrect email address format")
            .When(request => string.IsNullOrWhiteSpace(request.Phone) && string.IsNullOrWhiteSpace(request.Telegram));
        RuleFor(request => request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .Must(Validator.PhoneNumberValidator!)
            .WithMessage("Incorrect phone number format")
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Telegram));
        RuleFor(request => request.Telegram)
            .NotNull()
            .NotEmpty()
            .WithMessage("All contact details (email, phone, telegram) cannot be empty")
            .When(request => string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone));
        RuleFor(request => request.NeuroDifferences)
            .NotNull()
            .NotEmpty()
            .WithMessage("Neuro differences value cannot be null or empty");
        RuleFor(request => request.PsyQuery)
            .NotNull()
            .NotEmpty()
            .WithMessage("Psy query value cannot be null or empty");
        RuleFor(request => request.TherapyExperience)
            .NotNull()
            .NotEmpty()
            .WithMessage("Therapy experience value cannot be null or empty");
    }
}