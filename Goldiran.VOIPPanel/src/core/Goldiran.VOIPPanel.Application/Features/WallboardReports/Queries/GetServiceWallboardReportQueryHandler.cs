using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;

namespace Goldiran.VOIPPanel.Application.Features.AskCustomers.Queries;

public class GetServiceWallboardReportQueryHandler : IRequestHandler<GetServiceWallboardsQuery, ServiceWallboardReportDto>
{
    private readonly IServiceWallboardReportQueryService _serviceWallboardReportQueryService;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;

    public GetServiceWallboardReportQueryHandler(IMapper mapper, IServiceWallboardReportQueryService serviceWallboardReportQueryService, IReadModelContext readModelContext)
    {
        _mapper = mapper;
        _readModelContext = readModelContext;
        _serviceWallboardReportQueryService = serviceWallboardReportQueryService;
    }

    public async Task<ServiceWallboardReportDto> Handle(GetServiceWallboardsQuery request, CancellationToken cancellationToken)
    {
        

        return await _serviceWallboardReportQueryService.GetServiceWallboardsInfo(request);

    }


}
