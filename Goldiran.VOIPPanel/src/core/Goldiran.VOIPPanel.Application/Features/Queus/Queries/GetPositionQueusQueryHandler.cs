using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Queries;


public class GetPositionQueusQueryHandler : IRequestHandler<GetPositionQueusQuery, PaginatedList<QueuDto>>
{
    private readonly IMapper _mapper;
    private readonly IQueuQueryService _QueuQueryService;
    public GetPositionQueusQueryHandler(IMapper mapper, IQueuQueryService QueuQueryService)
    {
        _QueuQueryService = QueuQueryService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QueuDto>> Handle(GetPositionQueusQuery request, CancellationToken cancellationToken)
    {
        return await _QueuQueryService.GetPositionQueues(request);
    }
}
