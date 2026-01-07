using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Queries;

    public class GetPositionsQueryHandler : IRequestHandler<GetPositionsQuery, PaginatedList<PositionDto>>
    {
    private readonly IPositionQueryService _positionQueryService;
    private readonly IMapper _mapper;

        public GetPositionsQueryHandler(IMapper mapper, IPositionQueryService positionQueryService)
        {
            _mapper = mapper;
            _positionQueryService= positionQueryService;
        }

        public async Task<PaginatedList<PositionDto>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
        {

            return await _positionQueryService.GetPositions(request);
        }
    }
