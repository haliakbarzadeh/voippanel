using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.ReadModel.Mappers;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class UserQueryService : IUserQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public UserQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<UserDto> GetUserById(long userId)
    {
        var user = _context.User.AsNoTracking().FirstOrDefault(c => c.Id == userId);
        //var role =  Roles.Include(c => c.Claims).AsNoTracking().FirstOrDefault(c=>c.Id==roleId);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
    public async Task<PaginatedList<UserDto>> GetUsers(GetUsersQuery filter)
    {
        var result = await _context.User.AsNoTracking()
            .Where(c => (!filter.PersianFullName.IsNullOrEmpty() ? c.PersianFullName.Contains(filter.PersianFullName) : true) &&
            (!filter.NationalCode.IsNullOrEmpty() ? c.NationalCode.Contains(filter.NationalCode) : true) &&
            (!filter.UserName.IsNullOrEmpty() ? c.UserName.Contains(filter.UserName) : true) &&
            (filter.IsActive != null ? c.IsActive == filter.IsActive : true))
            .OrderByDescending(x => x.Id)
            .QueryEntityResult(filter)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(filter.PageNumber, filter.PageSize, filter.DisablePaging);

        return result;
    }
    public async Task<PaginatedList<UserRoleDto>> GetUserRoles(GetUserRolesQuery filter)
    {
        var result = await _context.UserRoles.AsNoTracking()
                                        .Include(c => c.User)
                                        .Include(c => c.Role)
                                        .Where(c => (filter.UserId != null ? c.UserId == filter.UserId : true) &&
                                                    (filter.RoleId != null ? c.RoleId == filter.RoleId : true))
                                        .OrderByDescending(x => x.Created)
                                        .QueryEntityResult(filter)
                                        .ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider)
                                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }
}
