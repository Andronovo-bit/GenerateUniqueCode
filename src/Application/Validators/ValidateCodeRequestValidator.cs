using FluentValidation;
using GenerateCampaignCode.Application.Requests;
using GenerateCampaignCode.Domain.Entities;
using Microsoft.Extensions.Options;

namespace GenerateCampaignCode.Application.Validators;

public class ValidateCodeRequestValidator : AbstractValidator<ValidateCodeRequest>
{
    public ValidateCodeRequestValidator(IOptions<CampaignCodeSettings> options)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Salt).NotEmpty().WithMessage("Salt is required."); // Added validation for unique key

        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
        RuleFor(x => x.Code).Length(options.Value.Length).WithMessage("Code must be 8 characters long.");
        RuleFor(x => x.Code).Matches($"^[{options.Value.Characters}]+$").WithMessage("Code must contain only valid characters.");
    }
}
