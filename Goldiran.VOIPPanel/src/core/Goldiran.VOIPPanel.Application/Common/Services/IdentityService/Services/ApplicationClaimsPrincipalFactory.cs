using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    /// <summary>
    /// Customizing claims transformation in ASP.NET Core Identity
    /// </summary>
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public static readonly string PhotoFileName = nameof(PhotoFileName);

        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IAppRoleManager _roleManager;
        private readonly IAppUserManager _userManager;

        public ApplicationClaimsPrincipalFactory(
            IAppUserManager userManager,
            IAppRoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base((UserManager<AppUser>)userManager, (RoleManager<AppRole>)roleManager, optionsAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public override async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user); // adds all `Options.ClaimsIdentity.RoleClaimType -> Role Claims` automatically + `Options.ClaimsIdentity.UserIdClaimType -> userId` & `Options.ClaimsIdentity.UserNameClaimType -> userName`
            addCustomClaims(user, principal);
            return principal;
        }

        private static void addCustomClaims(AppUser user, IPrincipal principal)
        {
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.GivenName, user.PersianFullName ?? string.Empty)
            });
        }
    }
}