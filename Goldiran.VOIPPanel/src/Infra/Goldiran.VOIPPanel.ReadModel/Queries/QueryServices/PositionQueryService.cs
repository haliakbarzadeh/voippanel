using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class PositionQueryService : IPositionQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public PositionQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PositionDto> GetPositionById(long id)
    {
        var entity=await _context.Position.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        return _mapper.Map<PositionDto>(entity);
    }

    public async Task<PaginatedList<PositionDto>> GetPositions(GetPositionsQuery filter)
    {
        var result = await _context.Position.Include(c=>c.ParentPosition).Include(c=>c.UserPositions).AsNoTracking()
                        .Where(c => (!filter.Title.IsNullOrEmpty() ? EF.Functions.Like(c.ContactedParentPositionName, $"%{filter.Title}%") : true) &&
                                    (!filter.PersianFullName.IsNullOrEmpty() ? c.UserPositions.Any(c=> EF.Functions.Like(c.User.PersianFullName, $"%{filter.PersianFullName}%")) : true) &&
                                    (filter.PositionType!=null ? c.PositionTypeId==filter.PositionType: true) &&
                                    (filter.IsRestricted && !filter.HasContentAccess ? EF.Functions.Like(c.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.Id) : true))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<PositionDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize,filter.DisablePaging);

        return result;
    }
}
