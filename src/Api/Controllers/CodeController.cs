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

    [HttpPost("generate")]
    public IActionResult GenerateCode([FromBody] GenerateCodeRequest request)
    {
        _logger.LogInformation("Generating code: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var code = _codeGenerator.GenerateCode(request.Id, request.Salt);
        return Ok(code);
    }

    [HttpPost("validate")]
    public IActionResult ValidateCode([FromBody] ValidateCodeRequest request)
    {
        _logger.LogInformation("Validating code: {Id} with unique key: {Salt}", request.Id, request.Salt);
        var isValid = _codeGenerator.ValidateCode(request.Id, request.Code, request.Salt);
        return Ok(isValid);
    }
}
