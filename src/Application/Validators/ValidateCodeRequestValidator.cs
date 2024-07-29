using FluentValidation;
using GenerateCampaignCode.Application.Requests;

namespace GenerateCampaignCode.Application.Validators;

public class ValidateCodeRequestValidator : AbstractValidator<ValidateCodeRequest>
{
    public ValidateCodeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Salt).NotEmpty().WithMessage("Salt is required."); // Added validation for unique key
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
    }
}
