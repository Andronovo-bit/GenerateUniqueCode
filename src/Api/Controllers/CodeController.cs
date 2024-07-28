using Microsoft.AspNetCore.Mvc;
using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Requests;

namespace GenerateCampaignCode.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeController : ControllerBase
{
    private readonly ICodeGenerator _codeGenerator;

    public CodeController(ICodeGenerator codeGenerator)
    {
        _codeGenerator = codeGenerator;
    }

    [HttpPost("generate")]
    public IActionResult GenerateCode([FromBody] GenerateCodeRequest request)
    {
        var code = _codeGenerator.GenerateCode(request.productId);
        return Ok(code);
    }

    [HttpPost("validate")]
    public IActionResult ValidateCode([FromBody] ValidateCodeRequest request)
    {
        var isValid = _codeGenerator.ValidateCode(request.productId, request.Code);
        return Ok(isValid);
    }
}
