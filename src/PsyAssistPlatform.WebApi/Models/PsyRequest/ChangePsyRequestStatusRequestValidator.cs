using FluentValidation;

namespace PsyAssistPlatform.WebApi.Models.PsyRequest;

public class ChangePsyRequestStatusRequestValidator : AbstractValidator<ChangePsyRequestStatusRequest>
{
    public ChangePsyRequestStatusRequestValidator()
    {
        RuleFor(request => request.PsyRequestId)
            .GreaterThan(0)
            .WithMessage("Psychological request Id value must be greater than 0");
        RuleFor(request => request.NewStatusId)
            .GreaterThan(1)
            .WithMessage("New status Id value must be greater than 1");
        RuleFor(request => request.PsychologistProfileId)
            .GreaterThan(0)
            .WithMessage("Psychologist Id value must be greater than 0");
        RuleFor(request => request.Comment)
            .NotNull()
            .NotEmpty()
            .WithMessage("Comment value cannot be null or empty");
    }
}