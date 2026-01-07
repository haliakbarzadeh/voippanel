namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;

public class GetTokenResponse
{
    public bool IsTemporary { get; set; }
    public long? UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool IsMultiPosition { get; set; } = false;
    public List<LoginPositions> PositionList { get; set; } = new List<LoginPositions>();
}

public class LoginPositions
{
    public long? Id { get; set; }
    public string Title { get; set; }

}