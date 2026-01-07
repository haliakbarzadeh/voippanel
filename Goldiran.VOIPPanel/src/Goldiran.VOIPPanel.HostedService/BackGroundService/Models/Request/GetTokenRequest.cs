namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;

public class GetTokenRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public long? PositionId { get; set; }
}
