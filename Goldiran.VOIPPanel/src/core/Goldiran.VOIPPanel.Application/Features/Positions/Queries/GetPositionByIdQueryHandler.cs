using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Queries;

public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, PositionDto>
{
    private readonly IMapper _mapper;
    private readonly IPositionQueryService _positionQueryService;

    public GetPositionByIdQueryHandler(IMapper mapper, IPositionQueryService positionQueryService)
    {
        _positionQueryService = positionQueryService;
        _mapper = mapper;
    }

    public async Task<PositionDto> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _positionQueryService.GetPositionById(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Position), request.Id);
        }

        return entity;
    }
}

