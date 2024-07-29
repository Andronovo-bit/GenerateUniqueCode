using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace GenerateCampaignCode.Application.Services;

public class CodeGenerator : ICodeGenerator
{
    private readonly CampaignCodeSettings _campaignCodeSettings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CodeGenerator> _logger;


    public CodeGenerator(IOptions<CampaignCodeSettings> options, IMemoryCache cache, ILogger<CodeGenerator> logger)
    {
        _campaignCodeSettings = options.Value;
        _cache = cache;
        _logger = logger;
    }

    public string GenerateCodeSHA1(string id, string salt)
    {
        var encodeStringKey = Encoding.UTF8.GetBytes($"{id}{salt}{_campaignCodeSettings.PrivateKey}");
        var cacheKey = Convert.ToBase64String(encodeStringKey);

        if (_cache.TryGetValue(cacheKey, out string cachedCode))
        {
            _logger.LogInformation("Returning cached code: {cachedCode}", cachedCode);
            return cachedCode ?? string.Empty;
        }

        ReadOnlySpan<char> characters = _campaignCodeSettings.Characters.AsSpan();
        int length = _campaignCodeSettings.Length;
        ReadOnlySpan<byte> hash = SHA1.HashData(encodeStringKey);

        //this code randomly selects a start point in the hash to use as the seed

        //var randomStart = new Random().Next(0, hash.Length - 4);
        //hash = hash.Slice(randomStart, hash.Length - randomStart);
        //int seed = BitConverter.ToInt32(hash);


        int seed = BitConverter.ToInt32(hash);
        var random = new Random(seed);

        Span<char> code = stackalloc char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = characters[random.Next(characters.Length)];
        }

        string generatedCode = new(code);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromDays(1));
        try
        {
            _cache.Set(cacheKey, generatedCode, cacheEntryOptions);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while caching code: {generatedCode}", generatedCode);
        }

        _logger.LogInformation("Generated new code: {generatedCode}", generatedCode);

        return generatedCode;
    }

    public bool ValidateCodeSHA1(string id, string salt, string code)
    {
        var generatedCode = GenerateCodeSHA1(id, salt);
        return string.Equals(generatedCode, code, StringComparison.OrdinalIgnoreCase);
    }

    public string GenerateCodeWithHMACSHA256(string id, string salt)
    {
        var encodeStringKey = Encoding.UTF8.GetBytes($"{id}{salt}");

        using (HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_campaignCodeSettings.PrivateKey)))
        {
            ReadOnlySpan<byte> hashBytes = hmacsha256.ComputeHash(encodeStringKey);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _campaignCodeSettings.Length; i++)
            {
                int modValue = hashBytes[i] % _campaignCodeSettings.Characters.Length;
                builder.Append(_campaignCodeSettings.Characters[modValue]);
            }
            return builder.ToString();
        }
    }

    public bool ValidateCodeHMACSHA256(string id, string salt, string code)
    {
        string newCode = GenerateCodeWithHMACSHA256(id, salt);

        return newCode == code;
    }

}
