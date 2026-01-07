using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.Operations.Queries;

    public class GetGroupOperationsQueryHandler : IRequestHandler<GetGroupOperationsQuery, PaginatedList<AggregateOperationDto>>
    {
    private readonly IOperationQueryService _OperationQueryService;
    private readonly IMapper _mapper;

        public GetGroupOperationsQueryHandler(IMapper mapper, IOperationQueryService OperationQueryService)
        {
            _mapper = mapper;
            _OperationQueryService= OperationQueryService;
        }

        public async Task<PaginatedList<AggregateOperationDto>> Handle(GetGroupOperationsQuery request, CancellationToken cancellationToken)
        {
        if (request.IsCurrentUser)
        {
            request.User = request.UserId;
            request.Position = request.PositionId;
        }

        return await _OperationQueryService.GetGroupOperations(request);
        }
    }
