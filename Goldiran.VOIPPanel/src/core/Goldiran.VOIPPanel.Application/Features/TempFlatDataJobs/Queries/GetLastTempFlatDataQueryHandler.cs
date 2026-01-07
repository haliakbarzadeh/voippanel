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

public class GetLastTempFlatDataQueryHandler : IRequestHandler<GetTempFlatDataJobLastQuery, TempFlatDataJobDto>
{
    private readonly IMapper _mapper;
    private readonly ITempFlatDataJobQueryService _TempFlatDataJobQueryService;

    public GetLastTempFlatDataQueryHandler(IMapper mapper, ITempFlatDataJobQueryService TempFlatDataJobQueryService)
    {
        _TempFlatDataJobQueryService = TempFlatDataJobQueryService;
        _mapper = mapper;
    }

    public async Task<TempFlatDataJobDto> Handle(GetTempFlatDataJobLastQuery request, CancellationToken cancellationToken)
    {
        var entity = await _TempFlatDataJobQueryService.GetLastFlatData(request);

        //if (entity == null)
        //{
        //    throw new NotFoundException(nameof(Operation), request.ReportType);
        //}

        return entity;
    }
}

