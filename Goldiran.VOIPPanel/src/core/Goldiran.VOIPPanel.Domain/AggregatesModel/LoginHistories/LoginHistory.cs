using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Enums;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
public class LoginHistory : AggregateRoot<long>
{
    public long UserId { get; private set; }
    public string? Ip { get; private set; }
    public bool Success { get; private set; }
    public LoginFunctionType LoginFunctionTypeId { get; private set; }
    public DateTime ActionDate { get; private set; }
    public LoginHistory(long userId, string? ip, bool success, LoginFunctionType loginFunctionTypeId, DateTime actionDate)
    {
        UserId = userId;
        Ip = ip;
        Success = success;
        LoginFunctionTypeId = loginFunctionTypeId;
        ActionDate = actionDate;
    }


    //public AppUser User { get; set; }
}
