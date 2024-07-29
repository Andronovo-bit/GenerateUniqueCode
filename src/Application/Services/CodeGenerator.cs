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

    public string? GenerateCode(string id, string salt)
    {
        var encodeStringKey = $"{id}{salt}{_campaignCodeSettings.PrivateKey}";
        var cacheKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(encodeStringKey));

        if (_cache.TryGetValue(cacheKey, out string cachedCode))
        {
            _logger.LogInformation("Returning cached code: {cachedCode}", cachedCode);
            return cachedCode;
        }

        ReadOnlySpan<char> characters = _campaignCodeSettings.Characters.AsSpan();
        int length = _campaignCodeSettings.Length;
        ReadOnlySpan<byte> hash = SHA512.HashData(Encoding.UTF8.GetBytes(encodeStringKey));

        int seed = BitConverter.ToInt32(hash.Slice(0, 4));
        var random = new Random(seed);

        Span<char> code = stackalloc char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = characters[random.Next(characters.Length)];
        }

        string generatedCode = new(code);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromDays(1));
        _cache.Set(cacheKey, generatedCode, cacheEntryOptions);

        _logger.LogInformation("Generated new code: {generatedCode}", generatedCode);

        return generatedCode;
    }

    public bool ValidateCode(string id, string code, string salt)
    {
        var generatedCode = GenerateCode(id, salt);
        return string.Equals(generatedCode, code, StringComparison.OrdinalIgnoreCase);
    }
}
