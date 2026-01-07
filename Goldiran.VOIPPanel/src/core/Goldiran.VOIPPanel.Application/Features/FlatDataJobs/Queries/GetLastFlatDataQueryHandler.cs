using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.IQueries;

namespace Goldiran.VOIPPanel.Application.Features.Operations.Queries;

public class GetLastFlatDataQueryHandler : IRequestHandler<GetFlatDataJobLastQuery, FlatDataJobDto>
{
    private readonly IMapper _mapper;
    private readonly IFlatDataJobQueryService _flatDataJobQueryService;

    public GetLastFlatDataQueryHandler(IMapper mapper, IFlatDataJobQueryService flatDataJobQueryService)
    {
        _flatDataJobQueryService = flatDataJobQueryService;
        _mapper = mapper;
    }

    public async Task<FlatDataJobDto> Handle(GetFlatDataJobLastQuery request, CancellationToken cancellationToken)
    {
        var entity = await _flatDataJobQueryService.GetLastFlatData(request);

        //if (entity == null)
        //{
        //    throw new NotFoundException(nameof(Operation), request.ReportType);
        //}

        return entity;
    }
}

