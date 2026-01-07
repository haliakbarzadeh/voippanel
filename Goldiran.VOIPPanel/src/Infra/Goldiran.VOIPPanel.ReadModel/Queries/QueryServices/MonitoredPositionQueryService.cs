using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class MonitoredPositionQueryService : IMonitoredPositionQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public MonitoredPositionQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<MonitoredPositionDto>> GetMonitoredPositionById(long id)
    {
        return await _context.MonitoredPosition.Include(c => c.SourcePosition).Include(c => c.DestPosition).AsNoTracking().Where(c => c.Id == id)
                            .OrderByDescending(x => x.Id)
                            .ProjectTo<MonitoredPositionDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();
    }

    public async Task<PaginatedList<MonitoredPositionDto>> GetMonitoredPositions(GetMonitoredPositionsQuery filter)
    {
        var result = await _context.MonitoredPosition.Include(c=>c.SourcePosition).Include(c=>c.DestPosition).AsNoTracking()
                        .Where(c => (filter.SourcePositionId!=null ? c.SourcePositionId==filter.SourcePositionId : true) &&
                            (filter.DestPositionId != null ? c.DestPositionId == filter.DestPositionId : true))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<MonitoredPositionDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<GroupMonitoredPositionDto>> GetGroupMonitoredPosition(GetGroupMonitoredPositionsQuery filter)
    {
        var result = await _context.MonitoredPosition.Include(c => c.SourcePosition).AsNoTracking()
                        .Where(c => (filter.SourcePositionId != null ? c.SourcePositionId == filter.SourcePositionId : true) &&
                            (filter.DestPositionId != null ? c.DestPositionId == filter.DestPositionId : true))
                      .OrderByDescending(x => x.Id)
                      .ProjectTo<MonitoredPositionDto>(_mapper.ConfigurationProvider)
                      //.Select(c=>new MonitoredPositionDto() { SourcePositionId=c.SourcePositionId,SourcePositionTitle=c.SourcePositionTitle})
                      .GroupBy(c => new { c.SourcePositionId, c.SourcePositionTitle })
                      .Select(c => new GroupMonitoredPositionDto() { SourcePositionId = c.Key.SourcePositionId, SourcePositionTitle = c.Key.SourcePositionTitle })
                      .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }


}
