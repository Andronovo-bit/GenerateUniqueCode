using FluentValidation;
using GenerateCampaignCode.Application.Requests;

namespace GenerateCampaignCode.Application.Validators;

public class GenerateCodeRequestValidator : AbstractValidator<GenerateCodeRequest>
{
    public GenerateCodeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Salt).NotEmpty().WithMessage("Salt is required."); // Added validation for unique key
    }
}
