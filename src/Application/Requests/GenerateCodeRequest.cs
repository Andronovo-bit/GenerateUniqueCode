using System.ComponentModel.DataAnnotations;

namespace GenerateCampaignCode.Application.Requests;

public class GenerateCodeRequest
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Salt { get; set; } // This is maybe timestamp or some other unique value
}
