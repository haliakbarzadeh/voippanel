using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public interface ITokenService
    {
        Task<Tuple<string, string>> GenerateAccessToken(AppUser user, long? positionId=null);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
