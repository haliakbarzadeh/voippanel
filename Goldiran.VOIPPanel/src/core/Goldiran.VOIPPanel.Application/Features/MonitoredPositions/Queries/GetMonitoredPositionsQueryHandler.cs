using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Queries;

public class GetMonitoredPositionsQueryHandler : IRequestHandler<GetMonitoredPositionsQuery, PaginatedList<MonitoredPositionDto>>
{
    private readonly IMonitoredPositionQueryService _MonitoredPositionQueryService; 
    public GetMonitoredPositionsQueryHandler(IMonitoredPositionQueryService MonitoredPositionQueryService)
    {
        _MonitoredPositionQueryService = MonitoredPositionQueryService;
    }

    public async Task<PaginatedList<MonitoredPositionDto>> Handle(GetMonitoredPositionsQuery request, CancellationToken cancellationToken)
    {

        return await _MonitoredPositionQueryService.GetMonitoredPositions(request);
    }
}
