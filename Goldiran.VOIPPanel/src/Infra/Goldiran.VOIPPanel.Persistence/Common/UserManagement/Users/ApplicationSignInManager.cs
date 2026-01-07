using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;



namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Users
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2578
    /// </summary>
    public class ApplicationSignInManager :
        SignInManager<AppUser>,
        IApplicationSignInManager
    {
        private readonly IAppUserManager _AppUserManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly ILogger<ApplicationSignInManager> _logger;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IUserConfirmation<AppUser> _confirmation;

        public ApplicationSignInManager(
            IAppUserManager AppUserManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<ApplicationSignInManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<AppUser> confirmation)
            : base((UserManager<AppUser>)AppUserManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes,null)
        {
            _AppUserManager = AppUserManager ?? throw new ArgumentNullException(nameof(AppUserManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _claimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(claimsFactory));
            _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _confirmation = confirmation;
        }

        #region BaseClass

        Task<bool> IApplicationSignInManager.IsLockedOut(AppUser AppUser)
        {
            return base.IsLockedOut(AppUser);
        }

        Task<SignInResult> IApplicationSignInManager.LockedOut(AppUser AppUser)
        {
            return base.LockedOut(AppUser);
        }

        Task<SignInResult> IApplicationSignInManager.PreSignInCheck(AppUser AppUser)
        {
            return base.PreSignInCheck(AppUser);
        }

        Task IApplicationSignInManager.ResetLockout(AppUser AppUser)
        {
            return base.ResetLockout(AppUser);
        }

        Task<SignInResult> IApplicationSignInManager.SignInOrTwoFactorAsync(AppUser AppUser, bool isPersistent, string loginProvider, bool bypassTwoFactor)
        {
            return base.SignInOrTwoFactorAsync(AppUser, isPersistent, loginProvider, bypassTwoFactor);
        }

        #endregion

        #region CustomMethods

        public bool IsCurrentUserSignedIn()
        {
            return IsSignedIn(_contextAccessor.HttpContext.User);
        }

        public Task<AppUser> ValidateCurrentUserSecurityStampAsync()
        {
            return ValidateSecurityStampAsync(_contextAccessor.HttpContext.User);
        }

        #endregion
    }
}
