using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.ReadModel.Mappers;



namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class RoleQueryService : IRoleQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public RoleQueryService(VOIPPanelReadModelContext context, IMapper mapper,  IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }

    public IList<RoleDto> FindCurrentUserRoles()
    {
        var userId = getCurrentUserId();
        return FindUserRoles(userId);
    }

    public IList<RoleDto> FindUserRoles(long userId)
    {
        //var userRolesQuery = from role in Roles
        //                     from user in role.Users
        //                     where user.UserId == userId
        //                     select role;
        //return userRolesQuery.OrderBy(x => x.Name).Select(c => _mapper.Map<AppRoleDto>(c)).ToList();

        return _context.Role.Include(x => x.Claims).Where(c => c.UserRoles.Select(c => c.UserId).Contains(userId)).Select(c => _mapper.Map<RoleDto>(c)).ToList();
    }

    public Task<List<RoleDto>> GetAllCustomRolesAsync()
    {
        return _context.Role.Select(c => _mapper.Map<RoleDto>(c)).ToListAsync();
    }

    public IList<RoleDto> GetRolesForCurrentUser()
    {
        var userId = getCurrentUserId();
        return GetRolesForUser(userId);
    }

    public IList<RoleDto> GetRolesForUser(long userId)
    {
        var roles = FindUserRoles(userId);
        if (roles == null || !roles.Any())
        {
            return (new List<RoleDto>());
        }

        return roles.ToList();
    }

    public IList<RoleDto> GetAllCustomRolesAndUsersCountList()
    {
        return _context.Role.Select(c => _mapper.Map<RoleDto>(c)).ToList();
    }

    public async Task<RoleDto> GetRoleById(long roleId)
    {
        var role = _context.Role.Include(c => c.UserRoles).ThenInclude(c=>c.User).AsNoTracking().FirstOrDefault(c => c.Id == roleId);
        //var role =  Roles.Include(c => c.Claims).AsNoTracking().FirstOrDefault(c=>c.Id==roleId);

        var rolesDto = _mapper.Map<RoleDto>(role);

        return rolesDto;
    }
    public async Task<PaginatedList<RoleDto>> GetRoles(GetRolesQuery filter)
    {
        var result = await _context.Role.Include(c=>c.UserRoles)
                .AsNoTracking()
                .Where(c => !filter.Name.IsNullOrEmpty() ? c.Name.Contains(filter.Name) : true)
                .OrderByDescending(x => x.Id) //Default Sort Order
                .QueryEntityResult(filter)
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(filter.PageNumber, filter.PageSize,true);

        return result;
    }

    private long getCurrentUserId() => _contextAccessor.HttpContext.User.Identity.GetUserId<long>();

}
