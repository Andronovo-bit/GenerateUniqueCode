namespace GenerateCampaignCode.Application.Requests;

public class GenerateCodeRequest
{
    public string Id { get; set; }
    public string Salt { get; set; } // This is maybe timestamp or some other unique value
}
