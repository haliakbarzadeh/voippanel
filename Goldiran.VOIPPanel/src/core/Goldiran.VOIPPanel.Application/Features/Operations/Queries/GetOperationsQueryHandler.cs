using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.Operations.Queries;

    public class GetOperationsQueryHandler : IRequestHandler<GetOperationsQuery, PaginatedList<OperationDto>>
    {
    private readonly IOperationQueryService _OperationQueryService;
    private readonly IMapper _mapper;

        public GetOperationsQueryHandler(IMapper mapper, IOperationQueryService OperationQueryService)
        {
            _mapper = mapper;
            _OperationQueryService= OperationQueryService;
        }

        public async Task<PaginatedList<OperationDto>> Handle(GetOperationsQuery request, CancellationToken cancellationToken)
        {
        if (request.IsCurrentUser)
            request.User = request.UserId;

        return await _OperationQueryService.GetOperations(request);
        }
    }
