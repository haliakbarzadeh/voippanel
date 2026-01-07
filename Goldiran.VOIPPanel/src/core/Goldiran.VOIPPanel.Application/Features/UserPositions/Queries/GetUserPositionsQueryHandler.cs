using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Queries;

public class GetUserPositionsQueryHandler : IRequestHandler<GetUserPositionsQuery, PaginatedList<UserPositionDto>>
{
    private readonly IUserPositionQueryService _userPositionQueryService; 
    public GetUserPositionsQueryHandler(IUserPositionQueryService userPositionQueryService)
    {
        _userPositionQueryService = userPositionQueryService;
    }

    public async Task<PaginatedList<UserPositionDto>> Handle(GetUserPositionsQuery request, CancellationToken cancellationToken)
    {

        return await _userPositionQueryService.GetUserPositions(request);
    }
}
