using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Queries;


public class GetGroupMonitoredPositionsQueryHandler : IRequestHandler<GetGroupMonitoredPositionsQuery, PaginatedList<GroupMonitoredPositionDto>>
{
    private readonly IMonitoredPositionQueryService _monitoredPositionQueryService;

    public GetGroupMonitoredPositionsQueryHandler(IMonitoredPositionQueryService monitoredPositionQueryService)
    {
        _monitoredPositionQueryService = monitoredPositionQueryService;
    }

    public async Task<PaginatedList<GroupMonitoredPositionDto>> Handle(GetGroupMonitoredPositionsQuery request, CancellationToken cancellationToken)
    {
        return await _monitoredPositionQueryService.GetGroupMonitoredPosition(request);   
    }
}

