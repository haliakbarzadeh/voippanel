using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;



namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Users
{
    /// <summary>

    /// </summary>
    public class AppUserStore :
        UserStore<AppUser, AppRole, VOIPPanelContext, long, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>,
        IAppUserStore
    {
        private readonly VOIPPanelContext _uow;
        private readonly IdentityErrorDescriber _describer;

        public AppUserStore(
            VOIPPanelContext uow,
            IdentityErrorDescriber describer)
            : base((VOIPPanelContext)uow, describer)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _describer = describer ?? throw new ArgumentNullException(nameof(_describer));
        }

        #region BaseClass

        protected override AppUserClaim CreateUserClaim(AppUser user, Claim claim)
        {
            var userClaim = new AppUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        protected override AppUserLogin CreateUserLogin(AppUser user, UserLoginInfo login)
        {
            return new AppUserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
        }

        protected override AppUserRole CreateUserRole(AppUser user, AppRole role)
        {
            return new AppUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }

        protected override AppUserToken CreateUserToken(AppUser user, string loginProvider, string name, string value)
        {
            return new AppUserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }

        Task IAppUserStore.AddUserTokenAsync(AppUserToken token)
        {
            return base.AddUserTokenAsync(token);
        }

        Task<AppRole> IAppUserStore.FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return base.FindRoleAsync(normalizedRoleName, cancellationToken);
        }

        Task<AppUserToken> IAppUserStore.FindTokenAsync(AppUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return base.FindTokenAsync(user, loginProvider, name, cancellationToken);
        }

        Task<AppUser> IAppUserStore.FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            return base.FindUserAsync(userId, cancellationToken);
        }

        Task<AppUserLogin> IAppUserStore.FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return base.FindUserLoginAsync(userId, loginProvider, providerKey, cancellationToken);
        }

        Task<AppUserLogin> IAppUserStore.FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return base.FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
        }

        Task<AppUserRole> IAppUserStore.FindUserRoleAsync(long userId, long roleId, CancellationToken cancellationToken)
        {
            return base.FindUserRoleAsync(userId, roleId, cancellationToken);
        }

        Task IAppUserStore.RemoveUserTokenAsync(AppUserToken token)
        {
            return base.RemoveUserTokenAsync(token);
        }

        #endregion

        #region CustomMethods

        // Add custom methods here

        #endregion
    }
}