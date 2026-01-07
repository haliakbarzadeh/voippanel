using Voip.Framework.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Goldiran.VOIPPanel.Api.Authorization
{
    public class PolicyAuthorizationHandler : AuthorizationHandler<AuthorizationPolicyRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PolicyAuthorizationHandler(IHttpContextAccessor httpContextAccessor) =>_httpContextAccessor= httpContextAccessor;
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationPolicyRequirements requirement)
        {
            //if (context.User.HasClaim(
            //c => c.Type == "permission" && c.Value == requirement.ClaimValue))
            if (context.User.HasClaim(
           c => c.Type == "permission" && !requirement.ClaimValue.IsNullOrEmpty() && requirement.ClaimValue.Contains(c.Value)))
            {
                // Code to check expiration date omitted for brevity.
                context.Succeed(requirement);
            }
            else
            {
                try
                {
                    if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && _httpContextAccessor.HttpContext.Response.StatusCode != 401 && _httpContextAccessor.HttpContext.Response.StatusCode != 400)
                    {
                        context.Fail();
                        _httpContextAccessor.HttpContext.Response.StatusCode = 401;
                        _httpContextAccessor.HttpContext.Response.ContentType = "text/plain";
                        return _httpContextAccessor.HttpContext.Response.WriteAsync("پایان زمان توکن");
                    }
                    if (_httpContextAccessor.HttpContext.Response.StatusCode == 200 || _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        context.Fail();
                        _httpContextAccessor.HttpContext.Response.StatusCode = 400;
                        _httpContextAccessor.HttpContext.Response.ContentType = "text/plain";
                        return _httpContextAccessor.HttpContext.Response.WriteAsync("عدم دسترسی به عملیات مورد نظر");
                    }
                }
                catch (Exception ex)
                {

                   
                }
             

                //_httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                //throw new InvalidOperationException("عدم دسترسی به عملیات مورد نظر");
            }

            return Task.CompletedTask;
        }
    }
}
