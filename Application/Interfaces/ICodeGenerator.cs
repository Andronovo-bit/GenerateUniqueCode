namespace GenerateCampaignCode.Application.Interfaces;

public interface ICodeGenerator
{
    string GenerateCode(string userId);
    bool ValidateCode(string userId, string code);
}
