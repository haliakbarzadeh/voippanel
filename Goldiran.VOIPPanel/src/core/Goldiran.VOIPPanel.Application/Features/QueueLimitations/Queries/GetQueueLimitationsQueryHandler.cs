using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Queries;

public class GetQueueLimitationsQueryHandler : IRequestHandler<GetQueueLimitationsQuery, PaginatedList<QueueLimitationDto>>
{
    private readonly IQueueLimitationQueryService _queueLimitationQueryService; 
    public GetQueueLimitationsQueryHandler(IQueueLimitationQueryService queueLimitationQueryService)
    {
        _queueLimitationQueryService = queueLimitationQueryService;
    }

    public async Task<PaginatedList<QueueLimitationDto>> Handle(GetQueueLimitationsQuery request, CancellationToken cancellationToken)
    {

        return await _queueLimitationQueryService.GetQueueLimitations(request);
    }
}
