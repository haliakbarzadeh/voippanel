using Microsoft.AspNetCore.Authorization;

namespace Goldiran.VOIPPanel.Api.Authorization;

public class AuthorizationPolicyRequirements:IAuthorizationRequirement
{
    // public string ClaimValue { get; }
    // public AuthorizationPolicyRequirements(string claimValue) =>
    //ClaimValue = claimValue;
    public IList<string> ClaimValue { get; }
    public AuthorizationPolicyRequirements(IList<string> claimValue) =>
   ClaimValue = claimValue;

}
