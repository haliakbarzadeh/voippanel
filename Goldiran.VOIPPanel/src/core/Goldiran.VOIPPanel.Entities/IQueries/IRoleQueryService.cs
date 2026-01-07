using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;

namespace Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;

public interface IRoleQueryService : IBaseQueryService
{
    Task<RoleDto> GetRoleById(long roleId);
    IList<RoleDto> FindUserRoles(long userId);
    Task<List<RoleDto>> GetAllCustomRolesAsync();
    IList<RoleDto> GetRolesForCurrentUser();
    IList<RoleDto> GetRolesForUser(long userId);
    IList<RoleDto> GetAllCustomRolesAndUsersCountList();
    Task<PaginatedList<RoleDto>> GetRoles(GetRolesQuery filter);


}
