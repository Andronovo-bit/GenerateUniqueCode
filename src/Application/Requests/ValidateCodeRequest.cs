namespace GenerateCampaignCode.Application.Requests;

public class ValidateCodeRequest
{
    public string Id { get; set; }
    public string Code { get; set; }

    public string Salt { get; set; } // This is maybe timestamp or some other unique value
}
