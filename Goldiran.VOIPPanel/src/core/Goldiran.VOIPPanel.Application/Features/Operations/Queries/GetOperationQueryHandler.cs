using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.Operations.Queries;

    public class GetOperationQueryHandler : IRequestHandler<GetOperationQuery, OperationDto>
    {
    private readonly IOperationQueryService _OperationQueryService;
    private readonly IMapper _mapper;

        public GetOperationQueryHandler(IMapper mapper, IOperationQueryService OperationQueryService)
        {
            _mapper = mapper;
            _OperationQueryService= OperationQueryService;
        }

        public async Task<OperationDto> Handle(GetOperationQuery request, CancellationToken cancellationToken)
        {
            if (request.IsCurrentUser)
            {
                request.User = request.UserId;
                request.Position = request.PositionId;
            }
            request.IsCurrentStatus = true;

            return await _OperationQueryService.GetOperation(request);
        }
    }
