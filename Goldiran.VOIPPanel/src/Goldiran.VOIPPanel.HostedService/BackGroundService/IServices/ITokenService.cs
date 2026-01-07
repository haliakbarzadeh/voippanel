

using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;

public interface ITokenService
{
    public Task<GetTokenResponse> GetToken();
}
