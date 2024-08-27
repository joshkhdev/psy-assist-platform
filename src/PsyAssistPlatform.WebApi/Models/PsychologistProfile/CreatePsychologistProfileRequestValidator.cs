using FluentValidation;

namespace PsyAssistPlatform.WebApi.Models.PsychologistProfile;

public class CreatePsychologistProfileRequestValidator : AbstractValidator<CreatePsychologistProfileRequest>
{
    public CreatePsychologistProfileRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name value cannot be null or empty");
        RuleFor(request => request.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("Description value cannot be null or empty");
        RuleFor(request => request.TimeZone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Time zone value cannot be null or empty");
        RuleFor(request => request.IncludingQueries)
            .NotNull()
            .NotEmpty()
            .WithMessage("Including queries value cannot be null or empty");
        RuleFor(request => request.ExcludingQueries)
            .NotNull()
            .NotEmpty()
            .WithMessage("Excluding queries value cannot be null or empty");
    }
}