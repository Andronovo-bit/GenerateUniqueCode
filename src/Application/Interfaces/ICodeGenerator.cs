namespace GenerateCampaignCode.Application.Interfaces;

public interface ICodeGenerator
{
    string GenerateCode(string id, string salt);
    bool ValidateCode(string id, string code, string salt);
}
