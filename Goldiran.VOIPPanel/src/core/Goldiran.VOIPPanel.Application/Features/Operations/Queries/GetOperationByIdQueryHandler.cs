using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.Operations.Queries;

public class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, OperationDto>
{
    private readonly IMapper _mapper;
    private readonly IOperationQueryService _OperationQueryService;

    public GetOperationByIdQueryHandler(IMapper mapper, IOperationQueryService OperationQueryService)
    {
        _OperationQueryService = OperationQueryService;
        _mapper = mapper;
    }

    public async Task<OperationDto> Handle(GetOperationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _OperationQueryService.GetOperationById(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Operation), request.Id);
        }

        return entity;
    }
}

