using FluentValidation;

namespace PsyAssistPlatform.WebApi.Models.Approach;

public class CreateApproachRequestValidator : AbstractValidator<CreateApproachRequest>
{
    public CreateApproachRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name value cannot be null or empty");
    }
}