using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Roles;

//private readonly TourismDbContext _context;
//public IUnitOfWork UnitOfWork { get { return _context; } }
//public CityRepository(TourismDbContext context)
//{
//    _context = context ?? throw new ArgumentNullException(nameof(context));
//}
/// <summary>
/// </summary>
public class AppRoleManager :
    RoleManager<AppRole>,
    IAppRoleManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly VOIPPanelContext _uow;
    private readonly IdentityErrorDescriber _errors;
    private readonly ILookupNormalizer _keyNormalizer;
    private readonly ILogger<AppRoleManager> _logger;
    private readonly IEnumerable<IRoleValidator<AppRole>> _roleValidators;
    private readonly IAppRoleStore _store;
    private readonly DbSet<AppUser> _users;
    private readonly DbSet<AppRole> _roles;
    private readonly IMapper _mapper;

    public AppRoleManager(
        IAppRoleStore store,
        IEnumerable<IRoleValidator<AppRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<AppRoleManager> logger,
        IHttpContextAccessor contextAccessor,
        VOIPPanelContext uow
        , IMapper mapper) :
        base((RoleStore<AppRole, VOIPPanelContext, long, AppUserRole, AppRoleClaim>)store, roleValidators, keyNormalizer, errors, logger)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _roleValidators = roleValidators ?? throw new ArgumentNullException(nameof(roleValidators));
        _keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(keyNormalizer));
        _errors = errors ?? throw new ArgumentNullException(nameof(errors));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _users = _uow.Users;
        _roles = _uow.Roles;
    }

    #region BaseClass

    #endregion

    #region CustomMethods


    //public IList<AppRoleAndUsersCountViewModel> GetAllCustomRolesAndUsersCountList()
    //{
    //    return Roles.Select(role =>
    //                            new RoleAndUsersCountViewModel
    //                            {
    //                                Role = role,
    //                                UsersCount = role.Users.Count()
    //                            }).ToList();
    //}

    //public async Task<PagedUsersListViewModel> GetPagedAppUsersInRoleListAsync(
    //        int roleId,
    //        int pageNumber, int recordsPerPage,
    //        string sortByField, SortOrder sortOrder,
    //        bool showAllUsers)
    //{
    //    var skipRecords = pageNumber * recordsPerPage;

    //    var roleUserIdsQuery = from role in Roles
    //                           where role.Id == roleId
    //                           from user in role.Users
    //                           select user.UserId;
    //    var query = _users.Include(user => user.Roles)
    //                      .Where(user => roleUserIdsQuery.Contains(user.Id))
    //                 .AsNoTracking();

    //    if (!showAllUsers)
    //    {
    //        query = query.Where(x => x.IsActive);
    //    }

    //    switch (sortByField)
    //    {
    //        case nameof(User.Id):
    //            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
    //            break;
    //        default:
    //            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
    //            break;
    //    }

    //    return new PagedUsersListViewModel
    //    {
    //        Paging =
    //        {
    //            TotalItems = await query.CountAsync()
    //        },
    //        Users = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
    //        Roles = await Roles.ToListAsync()
    //    };
    //}

    public IList<AppUser> GetAppUsersInRole(string roleName)
    {
        var roleUserIdsQuery = from role in Roles
                               where role.Name == roleName
                               from user in role.UserRoles
                               select user.UserId;
        return _users.Where(AppUser => roleUserIdsQuery.Contains(AppUser.Id))
                     .ToList();
    }

    public IList<AppUserRole> GetUserRolesInRole(string roleName)
    {
        return Roles.Where(role => role.Name == roleName)
                         .SelectMany(role => role.UserRoles)
                         .ToList();
    }

    public bool IsCurrentUserInRole(string roleName)
    {
        var userId = getCurrentUserId();
        return IsUserInRole(userId, roleName);
    }

    public bool IsUserInRole(long userId, string roleName)
    {
        var userRolesQuery = from role in Roles
                             where role.Name == roleName
                             from user in role.UserRoles
                             where user.UserId == userId
                             select role;
        var userRole = userRolesQuery.FirstOrDefault();
        return userRole != null;
    }

    public async Task<AppRole> FindRoleIncludeRoleClaimsAsync(long roleId)
    {
        return await Roles.Include(x => x.Claims).FirstOrDefaultAsync(x => x.Id == roleId);
    }

    public async Task<IdentityResult> AddOrUpdateRoleClaimsAsync(
        long roleId,
        string roleClaimType,
        IList<string> selectedRoleClaimValues)
    {
        var role = await FindRoleIncludeRoleClaimsAsync(roleId);
        if (role == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "RoleNotFound",
                Description = "نقش مورد نظر یافت نشد."
            });
        }

        var currentRoleClaimValues = role.Claims.Where(roleClaim => roleClaim.ClaimType == roleClaimType)
                                                .Select(roleClaim => roleClaim.ClaimValue)
                                                .ToList();

        if (selectedRoleClaimValues == null)
        {
            selectedRoleClaimValues = new List<string>();
        }
        var newClaimValuesToAdd = selectedRoleClaimValues.Except(currentRoleClaimValues).ToList();
        foreach (var claimValue in newClaimValuesToAdd)
        {
            role.Claims.Add(new AppRoleClaim
            {
                RoleId = role.Id,
                ClaimType = roleClaimType,
                ClaimValue = claimValue
            });
        }

        var removedClaimValues = currentRoleClaimValues.Except(selectedRoleClaimValues).ToList();
        foreach (var claimValue in removedClaimValues)
        {
            var roleClaim = role.Claims.SingleOrDefault(rc => rc.ClaimValue == claimValue &&
                                                              rc.ClaimType == roleClaimType);
            if (roleClaim != null)
            {
                role.Claims.Remove(roleClaim);
            }
        }

        return await UpdateAsync(role);
    }

    private long getCurrentUserId() => _contextAccessor.HttpContext.User.Identity.GetUserId<long>();

    //public async Task<PagedRolesListResponse> GetPagedRolesListAsync(GetRolesByFilterQuery model)
    //{
    //    var skipRecords = (model.PageNumber != null && model.PageSize != 0) ? ( model.PageNumber-1) * model.PageSize:null;

    //    var query = Roles.Include(c => c.Claims).AsNoTracking();

    //    if (model.Fk_TMSId != null)
    //    {
    //        query = query.Where(x => x.Fk_TMSId == model.Fk_TMSId);
    //    }

    //    if (model.Fk_TMSCoWorkerId != null)
    //    {
    //        query = query.Where(x => x.Fk_TMSCoWorkerId == model.Fk_TMSCoWorkerId);
    //    }

    //    if (!string.IsNullOrEmpty(model.Name))
    //    {
    //        query = query.Where(x => x.Name.Contains(model.Name));
    //    }

    //    var count = query.Count();
    //    query = query.OrderBy(x => x.Id);

    //    var roles = (count != 0 ? ((model.PageNumber != null && model.PageSize != 0) ? await query.Skip(Convert.ToInt32(skipRecords)).Take(Convert.ToInt32(model.PageSize)).ToListAsync() : await query.ToListAsync()) : null);
    //    var rolesDto = (count != 0 ? roles.Select(c => _mapper.Map<AppRoleDto>(c)).ToList() : null);

    //    return new PagedRolesListResponse()
    //    {
    //        Roles = rolesDto,
    //        Count = count
    //    };
    //}

    public async Task<IdentityResult> AddClaimAsync(long roleId, Claim claim)
    {
        var role = await FindByIdAsync(roleId.ToString());
        return await base.AddClaimAsync(role, claim);
    }

    public async Task<IdentityCommonResponse> AddClaimsAsync(long roleId, IEnumerable<Claim> claims)
    {
        var role = Roles.Where(c => c.Id == roleId).Include(c => c.Claims).First();
        //var deletedClaims =await RemoveClaimsAsync(roleId, role.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));
        var result = await _store.AddClaimsAsync(role, claims);
        if (result == 0)
        {
            return new IdentityCommonResponse()
            {
                Succeeded = false,
                Errors = new List<string>() { "خطا در هنگام ثبت داده ها" }
            };
        }
        else
        {
            return new IdentityCommonResponse()
            {
                Succeeded = true,
            };
        }
    }

    public async Task<IdentityResult> RemoveClaimAsync(long roleId, Claim claim)
    {
        var role = await FindByIdAsync(roleId.ToString());
        return await base.RemoveClaimAsync(role, claim);
    }

    public async Task<IdentityCommonResponse> RemoveClaimsAsync(long roleId, IEnumerable<Claim> claims)
    {
        var role = await FindByIdAsync(roleId.ToString());

        var result = await _store.RemoveClaimsAsync(role, claims);
        if (result == 0)
        {
            return new IdentityCommonResponse()
            {
                Succeeded = false,
                Errors = new List<string>() { "خطا در هنگام  حذف داده ها" }
            };
        }
        else
        {
            return new IdentityCommonResponse()
            {
                Succeeded = true,
            };
        }
    }

    public async Task<IdentityResult> AddOrUpdateUserRolesAsync(long roleId, IList<long> selectedUserIds, Action<AppUser> action = null)
    {
        var currentUserRoleIds = await _uow.UserRoles.Where(C => C.RoleId == roleId).Select(c => c.UserId).ToListAsync();
        if (selectedUserIds == null)
        {
            selectedUserIds = new List<long>();
        }
        var newRolesToAdd = selectedUserIds.Except(currentUserRoleIds).ToList();
        _uow.UserRoles.AddRange(newRolesToAdd.Select(c => new AppUserRole() { UserId = c, RoleId = roleId }));

        var removedRoles = currentUserRoleIds.Except(selectedUserIds).ToList();
        var userRolesRemoved = _uow.UserRoles.Where(c => c.RoleId == roleId && removedRoles.Contains(c.UserId));
        _uow.UserRoles.RemoveRange(userRolesRemoved);

        await _uow.SaveChangesAsync(new CancellationToken());
        return IdentityResult.Success;
    }
    //public async Task<IdentityResult> AddOrUpdateUserRolesAsync(long roleId, IList<long> selectedUserIds, Action<AppUser> action = null)
    //{
    //    var currentUserRoleIds = await _uow.AppRoleAssignments.Where(C => C.RoleId == roleId && C.EntityType==EntityTypeEnum.User).Select(c => c.EntityId).ToListAsync();
    //    if (selectedUserIds == null)
    //    {
    //        selectedUserIds = new List<long>();
    //    }
    //    var newRolesToAdd =_uow.AppUsers.Where(c=> selectedUserIds.Except(currentUserRoleIds).ToList().Contains(c.Id)).ToList();
    //    _uow.AppRoleAssignments.AddRange(newRolesToAdd.Select(c => new AppRoleAssignment() { EntityId = c.Id,EntityTitle=c.PersianFullName, RoleId = roleId,EntityType=EntityTypeEnum.User }));

    //    var removedRoles = currentUserRoleIds.Except(selectedUserIds).ToList();
    //    var userRolesRemoved = _uow.AppRoleAssignments.Where(c => c.RoleId == roleId && removedRoles.Contains(c.EntityId) && c.EntityType==EntityTypeEnum.User);
    //    _uow.AppRoleAssignments.RemoveRange(userRolesRemoved);

    //    await _uow.SaveChangesAsync(new CancellationToken());
    //    return IdentityResult.Success;
    //}
    //public async Task<IdentityResult> AddOrUpdateDeptRolesAsync(long roleId, IList<long> selectedUserIds, Action<AppUser> action = null)
    //{
    //    var currentUserRoleIds = await _uow.AppRoleAssignments.Where(C => C.RoleId == roleId && C.EntityType == EntityTypeEnum.Department).Select(c => c.EntityId).ToListAsync();
    //    if (selectedUserIds == null)
    //    {
    //        selectedUserIds = new List<long>();
    //    }
    //    var newRolesToAdd = _uow.Departments.Where(c => selectedUserIds.Except(currentUserRoleIds).ToList().Contains(c.Id)).ToList();
    //    _uow.AppRoleAssignments.AddRange(newRolesToAdd.Select(c => new AppRoleAssignment() { EntityId = c.Id, EntityTitle = c.Title, RoleId = roleId, EntityType = EntityTypeEnum.Department }));

    //    var removedRoles = currentUserRoleIds.Except(selectedUserIds).ToList();
    //    var userRolesRemoved = _uow.AppRoleAssignments.Where(c => c.RoleId == roleId && removedRoles.Contains(c.EntityId) && c.EntityType == EntityTypeEnum.Department);
    //    _uow.AppRoleAssignments.RemoveRange(userRolesRemoved);

    //    await _uow.SaveChangesAsync(new CancellationToken());
    //    return IdentityResult.Success;
    //}
    //public async Task<IdentityResult> AddOrUpdatePosRolesAsync(long roleId, IList<long> selectedUserIds, Action<AppUser> action = null)
    //{
    //    var currentUserRoleIds = await _uow.AppRoleAssignments.Where(C => C.RoleId == roleId && C.EntityType == EntityTypeEnum.Position).Select(c => c.EntityId).ToListAsync();
    //    if (selectedUserIds == null)
    //    {
    //        selectedUserIds = new List<long>();
    //    }
    //    var newRolesToAdd = _uow.Positions.Where(c => selectedUserIds.Except(currentUserRoleIds).ToList().Contains(c.Id)).ToList();
    //    _uow.AppRoleAssignments.AddRange(newRolesToAdd.Select(c => new AppRoleAssignment() { EntityId = c.Id, EntityTitle = c.Title, RoleId = roleId, EntityType = EntityTypeEnum.Position }));

    //    var removedRoles = currentUserRoleIds.Except(selectedUserIds).ToList();
    //    var userRolesRemoved = _uow.AppRoleAssignments.Where(c => c.RoleId == roleId && removedRoles.Contains(c.EntityId) && c.EntityType == EntityTypeEnum.Position);
    //    _uow.AppRoleAssignments.RemoveRange(userRolesRemoved);

    //    await _uow.SaveChangesAsync(new CancellationToken());
    //    return IdentityResult.Success;
    //}
    
    public async Task<IdentityResult> ChangeAccessesAsync(long roleId, List<string> accesses)
    {
        var currentPermissions=await _uow.RoleClaims.Where(c=>c.ClaimType=="Permission" && c.RoleId==roleId).Select(c=>c.ClaimValue).ToListAsync();

        if (accesses == null)
        {
            accesses = new List<string>();
        }
        List<string> addedAccessList=new List<string>();
        foreach (var c in accesses) 
        {
            ChangeRecursivePermission(c,addedAccessList);
        }

        var newAccessToAdd = addedAccessList.Except(currentPermissions).ToList();
        _uow.RoleClaims.AddRange(newAccessToAdd.Select(c => new AppRoleClaim() { ClaimType="Permission",ClaimValue=c, RoleId = roleId }));

        var removedAccess = currentPermissions.Except(addedAccessList).ToList();
        var accessRemoved = _uow.RoleClaims.Where(c => c.RoleId == roleId && removedAccess.Contains(c.ClaimValue) && c.ClaimType=="Permission");
        _uow.RoleClaims.RemoveRange(accessRemoved);

        await _uow.SaveChangesAsync(new CancellationToken());
        return IdentityResult.Success;
    }

    public async Task<List<PermissionNodes>> GetRolePermission(long roleId)
    {
        var list = await _uow.RoleClaims
                .AsNoTracking()
                .Where(c => c.RoleId == roleId && c.ClaimType == "permission")
                .Select(c => c.ClaimValue)
                .ToListAsync();


        var permissionNodesList = PermissionDefinitionResponse.PermissionNodes.Select(c => new PermissionNodes()
        {
            Checked = !list.IsNullOrEmpty() ? list.Contains(c.Name) : false,
            Description = c.Description,
            NodeId = c.NodeId,
            Order = c.Order,
            ParentNodeId = c.ParentNodeId,
            Name = c.Name
        }).ToList();
        //this section change only for front
        //if (permissionNodesList.Any())
        //{
        //    permissionNodesList = SetRecursivePermission(permissionNodesList);
        //}
        //

        return permissionNodesList;
    }


    #endregion

    #region Private Methods
    private List<PermissionNodes> SetRecursivePermission(List<PermissionNodes> permissionNodes)
    {
        var parentNodeIds = permissionNodes.Where(c => !c.Checked).Select(c => c.ParentNodeId).Distinct();

        var permissionNodesTemp = permissionNodes.Where(c => parentNodeIds.Contains(c.NodeId)).Select(c => c.NodeId).ToList();
        foreach (var item in permissionNodesTemp)
        {
            var permission = permissionNodes.Where((c) => c.NodeId == item).FirstOrDefault();
            permission.Checked = false;
        }
        return permissionNodes;

    }

    private void ChangeRecursivePermission(string access, List<string> addedAccessList)
    {
        var node= PermissionDefinitionResponse.PermissionNodes.FirstOrDefault(c=>c.Name == access);
        if (node != null && node.ParentNodeId != null)
        {
            if (!addedAccessList.Contains(access))
            {
                addedAccessList.Add(access);
            }

            if (node.ParentNodeId != null)
            {
                var parentNode = PermissionDefinitionResponse.PermissionNodes.FirstOrDefault(c => c.NodeId == node.ParentNodeId);

                if (!addedAccessList.Contains(parentNode.Name))
                {
                    addedAccessList.Add(parentNode.Name);
                    ChangeRecursivePermission(parentNode.Name, addedAccessList);
                }
                else
                {
                    ChangeRecursivePermission(parentNode.Name, addedAccessList);
                }
            }
        }
        

    }
    #endregion
}
