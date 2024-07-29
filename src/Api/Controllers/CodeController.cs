using Microsoft.AspNetCore.Mvc;
using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Requests;

namespace GenerateCampaignCode.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeController : ControllerBase
{
    private readonly ICodeGenerator _codeGenerator;
    private readonly ILogger<CodeController> _logger;


    public CodeController(ICodeGenerator codeGenerator, ILogger<CodeController> logger)
    {
        _codeGenerator = codeGenerator;
        _logger = logger;
    }

    [HttpPost("GenerateCodeWithSHA1")]
    public IActionResult GenerateCode([FromBody] GenerateCodeRequest request)
    {
        _logger.LogInformation("Generating code with HMACSHA1: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var code = _codeGenerator.GenerateCodeSHA1(request.Id, request.Salt);
        return Ok(code);
    }

    [HttpPost("ValidateCodeWithSHA1")]
    public IActionResult ValidateCode([FromBody] ValidateCodeRequest request)
    {
        _logger.LogInformation("Validating code with HMACSHA1: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var isValid = _codeGenerator.ValidateCodeSHA1(request.Id, request.Salt, request.Code);
        return Ok(isValid);
    }

    [HttpPost("GenerateCodeWithHMACSHA256")]
    public IActionResult GenerateCodeWithHMACSHA256([FromBody] GenerateCodeRequest request)
    {
        _logger.LogInformation("Generating code with HMACSHA256: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var code = _codeGenerator.GenerateCodeWithHMACSHA256(request.Id, request.Salt);
        return Ok(code);
    }

    [HttpPost("ValidateCodeWithHMACSHA256")]
    public IActionResult ValidateCodeWithHMACSHA256([FromBody] ValidateCodeRequest request)
    {
        _logger.LogInformation("Validating code with HMACSHA256: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var isValid = _codeGenerator.ValidateCodeHMACSHA256(request.Id, request.Salt, request.Code);
        return Ok(isValid);
    }
}
