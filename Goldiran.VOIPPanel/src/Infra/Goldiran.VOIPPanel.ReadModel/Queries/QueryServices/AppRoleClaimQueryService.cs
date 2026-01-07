using AutoMapper;
using AutoMapper.QueryableExtensions;
using Goldiran.Framework.Domain.Extensions;
using Goldiran.Framework.Domain.Models.CQRS;
using Goldiran.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.Common.Dtos;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.Dtos;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.Queries.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Queries.QueryServices;

public class AppRoleClaimQueryService : IAppRoleClaimQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public AppRoleClaimQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PositionDto> GetPositionById(long id)
    {
        var entity=await _context.Positions.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        return _mapper.Map<PositionDto>(entity);
    }

    public async Task<PaginatedList<PositionDto>> GetPositions(GetPositionsQuery filter)
    {
        var result = await _context.Positions.Include(c=>c.Users).AsNoTracking()
                        .Where(c => (!filter.Title.IsNullOrEmpty() ? c.ContactedParentPositionName.Contains(filter.Title) : true))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<PositionDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<List<AppRoleClaim>> GetAppRoleClaims(GetAppRoleClaimsQuery filter)
    {
        var result = await _context.AppRoleClaims.AsNoTracking()
            .Where(c=>(filter.UserId!=null?c.Role.UserRoles.Any(d=>d.UserId==filter.UserId):true) &&
                      (filter.RoleId != null ? c.RoleId==filter.RoleId : true))
                .ToListAsync();

        return result;
    }
}
