using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Common.Exceptions;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Queries;


public class GetUserPositionByIdQueryHandler : IRequestHandler<GetUserPositionByIdQuery, UserPositionDto>
{
    private readonly IUserPositionQueryService _userPositionQueryService;

    public GetUserPositionByIdQueryHandler(IUserPositionQueryService userPositionQueryService)
    {
        _userPositionQueryService = userPositionQueryService;
    }

    public async Task<UserPositionDto> Handle(GetUserPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userPositionQueryService.GetUserPositionById(request.Id);   

        if (entity == null)
        {
            throw new NotFoundException(nameof(UserPosition), request.Id);
        }

        return entity;
    }
}

