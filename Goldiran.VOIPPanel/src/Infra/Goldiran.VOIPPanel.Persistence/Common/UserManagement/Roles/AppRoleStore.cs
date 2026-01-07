using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Roles;

/// <summary>

/// </summary>
public class AppRoleStore :
    RoleStore<AppRole, VOIPPanelContext, long, AppUserRole, AppRoleClaim>,
    IAppRoleStore
{
    private readonly VOIPPanelContext _uow;
    private readonly IdentityErrorDescriber _describer;

    public AppRoleStore(
        VOIPPanelContext uow,
        IdentityErrorDescriber describer)
        : base((VOIPPanelContext)uow, describer)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _describer = describer ?? throw new ArgumentNullException(nameof(_describer));
    }

    public async Task<int> AddClaimsAsync(AppRole role, IEnumerable<Claim> claims)
    {
        var roleClaims = claims.Select(c => new AppRoleClaim() { RoleId = role.Id, ClaimType = c.Type, ClaimValue = c.Value });
        _uow.RoleClaims.AddRange(roleClaims);
        return await _uow.SaveChangesAsync(new CancellationToken());
    }


    public async Task<int> RemoveClaimsAsync(AppRole role, IEnumerable<Claim> claims)
    {
        //var roleClaims = claims.Select(c => new RoleClaim() { RoleId = role.Id, ClaimType = c.Type, ClaimValue = c.Value });
        var roleobj = Roles.Where(c => c.Id ==role.Id).Include(c => c.Claims).First();
        var roleClaims = role.Claims.Where(c => claims.Where(d => d.Type == c.ClaimType && d.Value == c.ClaimValue).Any());
        _uow.RoleClaims.RemoveRange(roleClaims);
        return await _uow.SaveChangesAsync(new CancellationToken());
    }

    #region BaseClass

    protected override AppRoleClaim CreateRoleClaim(AppRole role, Claim claim)
    {
        return new AppRoleClaim
        {
            RoleId = role.Id,
            ClaimType = claim.Type,
            ClaimValue = claim.Value
        };
    }

    #endregion

    #region CustomMethods

    #endregion
}