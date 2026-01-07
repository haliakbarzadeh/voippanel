using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens;
public class Token : AggregateRoot<long>
{
    public long UserId { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? TokenValue { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public Token(long userId, string? refreshToken, string? tokenValue, DateTime? refreshTokenExpiryTime)
    {
        UserId = userId;
        RefreshToken = refreshToken;
        TokenValue = tokenValue;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }

    public void Update(long userId, string? refreshToken, string? tokenValue, DateTime? refreshTokenExpiryTime)
    {
        UserId = userId;
        RefreshToken = refreshToken;
        TokenValue = tokenValue;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}
