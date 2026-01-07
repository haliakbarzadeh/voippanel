using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Domain.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.Common.Contracts
{
    public interface IAppRoleManager : IDisposable
    {
        #region BaseClass

        /// <summary>
        /// Gets an IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}"/>,
        /// otherwise throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <value>An IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}"/>.</value>
        /// <exception cref="NotSupportedException">Thrown if the persistence store is not an <see cref="IQueryableRoleStore{TRole}"/>.</exception>
        /// <remarks>
        /// Callers to this property should use <see cref="SupportsQueryableRoles"/> to ensure the backing role store supports
        /// returning an IQueryable list of roles.
        /// </remarks>
        IQueryable<AppRole> Roles { get; }

        /// <summary>
        /// Gets the normalizer to use when normalizing role names to keys.
        /// </summary>
        /// <value>
        /// The normalizer to use when normalizing role names to keys.
        /// </value>
        ILookupNormalizer KeyNormalizer { get; set; }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages.
        /// </summary>
        /// <value>
        /// The <see cref="IdentityErrorDescriber"/> used to provider error messages.
        /// </value>
        IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// Gets a list of validators for roles to call before persistence.
        /// </summary>
        /// <value>A list of validators for roles to call before persistence.</value>
        IList<IRoleValidator<AppRole>> RoleValidators { get; }

        /// <summary>
        /// Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        ILogger Logger { get; set; }

        /// <summary>
        /// Gets a flag indicating whether the underlying persistence store supports returning an <see cref="IQueryable"/> collection of roles.
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports returning an <see cref="IQueryable"/> collection of roles, otherwise false.
        /// </value>
        bool SupportsQueryableRoles { get; }

        /// <summary>
        /// Gets a flag indicating whether the underlying persistence store supports <see cref="Claim"/>s for roles.
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports <see cref="Claim"/>s for roles, otherwise false.
        /// </value>
        bool SupportsRoleClaims { get; }

        /// <summary>
        /// Adds a claim to a role.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        Task<IdentityResult> AddClaimAsync(AppRole role, Claim claim);

        /// <summary>
        /// Creates the specified <paramref name="role"/> in the persistence store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task<IdentityResult> CreateAsync(AppRole role);

        /// <summary>
        /// Deletes the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.
        /// </returns>
        Task<IdentityResult> DeleteAsync(AppRole role);

        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleId"/> if any.
        /// </summary>
        /// <param name="roleId">The role ID whose role should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the role
        /// associated with the specified <paramref name="roleId"/>
        /// </returns>
        Task<AppRole> FindByIdAsync(string roleId);

        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleName"/> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the role
        /// associated with the specified <paramref name="roleName"/>
        /// </returns>
        Task<AppRole> FindByNameAsync(string roleName);

        /// <summary>
        /// Gets a list of claims associated with the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose claims should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="Claim"/>s
        /// associated with the specified <paramref name="role"/>.
        /// </returns>
        Task<IList<Claim>> GetClaimsAsync(AppRole role);

        /// <summary>
        /// Gets a normalized representation of the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The value to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key"/>.</returns>
        string NormalizeKey(string key);

        /// <summary>
        /// Removes a claim from a role.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        Task<IdentityResult> RemoveClaimAsync(AppRole role, Claim claim);

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="roleName"/> exists.
        /// </summary>
        /// <param name="roleName">The role name whose existence should be checked.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing true if the role name exists, otherwise false.
        /// </returns>
        Task<bool> RoleExistsAsync(string roleName);

        /// <summary>
        /// Updates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.
        /// </returns>
        Task<IdentityResult> UpdateAsync(AppRole role);

        /// <summary>
        /// Updates the normalized name for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose normalized name needs to be updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task UpdateNormalizedRoleNameAsync(AppRole role);

        /// <summary>
        /// Gets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the name of the
        /// specified <paramref name="role"/>.
        /// </returns>
        Task<string> GetRoleNameAsync(AppRole role);

        /// <summary>
        /// Sets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="name">The name to set.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        Task<IdentityResult> SetRoleNameAsync(AppRole role, string name);

        /// <summary>
        /// Gets the ID of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the
        /// specified <paramref name="role"/>.
        /// </returns>
        Task<string> GetRoleIdAsync(AppRole role);
        #endregion

        #region CustomMethods
         IList<AppUser> GetAppUsersInRole(string roleName);
        IList<AppUserRole> GetUserRolesInRole(string roleName);

        bool IsCurrentUserInRole(string roleName);

        bool IsUserInRole(long userId, string roleName);

        //Task<PagedRolesListResponse> GetPagedRolesListAsync(GetRolesByFilterQuery model);

        //Task<PagedUsersListViewModel> GetPagedAppUsersInRoleListAsync(
        //        int roleId,
        //        int pageNumber, int recordsPerPage,
        //        string sortByField, SortOrder sortOrder,
        //        bool showAllUsers);

        Task<AppRole> FindRoleIncludeRoleClaimsAsync(long roleId);

        Task<IdentityResult> AddOrUpdateRoleClaimsAsync(
            long roleId,
            string roleClaimType,
            IList<string> selectedRoleClaimValues);
        Task<IdentityResult> AddOrUpdateUserRolesAsync(long roleId, IList<long> selectedUserIds, Action<AppUser> action = null);
        Task<IdentityResult> AddClaimAsync(long roleId, Claim claim);
        Task<IdentityCommonResponse> AddClaimsAsync(long roleId, IEnumerable<Claim> claims);
        Task<IdentityResult> RemoveClaimAsync(long roleId, Claim claim);
        Task<IdentityCommonResponse> RemoveClaimsAsync(long roleId, IEnumerable<Claim> claims);
        Task<IdentityResult> ChangeAccessesAsync(long roleId, List<string> accesses);
        Task<List<PermissionNodes>> GetRolePermission(long roleId);
        #endregion
    }
}