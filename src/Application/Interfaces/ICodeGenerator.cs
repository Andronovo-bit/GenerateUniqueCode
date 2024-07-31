using System;

namespace GenerateCampaignCode.Application.Interfaces;

public interface ICodeGenerator
{

    /// <summary>
    /// Generates a unique code using the SHA1 algorithm.
    /// </summary>
    /// <param name="id">A string representing an identifier.</param>
    /// <param name="salt">A string used to add complexity to the input.</param>
    /// <returns>A unique code as a string, generated based on the input id and salt.</returns>
    /// <remarks>
    /// The generated code is cached for 1 day
    /// </remarks>
    /// <example>
    /// This sample shows how to call the <see cref="GenerateCodeSHA1"/> method.
    /// <code>
    /// var code = GenerateCode SHA1("id", "salt");
    /// </code>
    /// </example>
    string GenerateCodeSHA1(string id, string salt);
    /// <summary>
    /// Generates a unique code using the HMACSHA256 algorithm.
    /// </summary>
    /// <param name="id">A string representing an identifier.</param>
    /// <param name="salt">A string used to add complexity to the input.</param>
    /// <returns>A unique code as a string, generated based on the input id and salt.</returns>
    /// <example>
    /// This sample shows how to call the <see cref="GenerateCodeWithHMACSHA256"/> method.
    /// <code>
    /// var code = GenerateCodeWithHMACSHA256("id", "salt");
    /// </code>
    /// </example>
    string GenerateCodeWithHMACSHA256(string id, string salt);
    bool ValidateCodeSHA1(string id, string salt, string code);
    bool ValidateCodeHMACSHA256(string id, string salt, string code);
}