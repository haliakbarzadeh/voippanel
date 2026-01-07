using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;

namespace Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;

public interface IUserQueryService : IBaseQueryService
{
    Task<UserDto> GetUserById(long UserId);
    Task<PaginatedList<UserDto>> GetUsers(GetUsersQuery filter);
    Task<PaginatedList<UserRoleDto>> GetUserRoles(GetUserRolesQuery filter);
}
