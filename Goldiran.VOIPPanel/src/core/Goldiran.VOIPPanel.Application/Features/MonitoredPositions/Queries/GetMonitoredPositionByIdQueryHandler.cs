using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Queries;

public class GetMonitoredPositionByIdQueryHandler : IRequestHandler<GetMonitoredPositionByIdQuery, List<MonitoredPositionDto>>
{
    private readonly IMonitoredPositionQueryService _MonitoredPositionQueryService; 
    public GetMonitoredPositionByIdQueryHandler(IMonitoredPositionQueryService MonitoredPositionQueryService)
    {
        _MonitoredPositionQueryService = MonitoredPositionQueryService;
    }

    public async Task<List<MonitoredPositionDto>> Handle(GetMonitoredPositionByIdQuery request, CancellationToken cancellationToken)
    {

        return await _MonitoredPositionQueryService.GetMonitoredPositionById(request.Id);
    }
}
