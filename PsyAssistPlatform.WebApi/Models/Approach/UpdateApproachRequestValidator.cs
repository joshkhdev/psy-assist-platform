using FluentValidation;

namespace PsyAssistPlatform.WebApi.Models.Approach;

public class UpdateApproachRequestValidator : AbstractValidator<UpdateApproachRequest>
{
    public UpdateApproachRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name value cannot be null or empty");
    }
}