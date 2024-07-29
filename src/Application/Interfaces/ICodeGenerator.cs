namespace GenerateCampaignCode.Application.Interfaces;

public interface ICodeGenerator
{
    string GenerateCodeSHA1(string id, string salt);
    string GenerateCodeWithHMACSHA256(string id, string salt);
    bool ValidateCodeSHA1(string id, string salt, string code);
    bool ValidateCodeHMACSHA256(string id, string salt, string code);
}