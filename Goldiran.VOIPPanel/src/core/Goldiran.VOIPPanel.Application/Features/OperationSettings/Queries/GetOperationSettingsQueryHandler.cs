using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.Contracts;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.OperationSettings.Queries;

public class GetOperationSettingsQueryHandler : IRequestHandler<GetOperationSettingsQuery, PaginatedList<OperationSettingDto>>
{
    private readonly IOperationSettingQueryService _operationSettingQueryService; 
    public GetOperationSettingsQueryHandler(IOperationSettingQueryService operationSettingQueryService)
    {
        _operationSettingQueryService = operationSettingQueryService;
    }

    public async Task<PaginatedList<OperationSettingDto>> Handle(GetOperationSettingsQuery request, CancellationToken cancellationToken)
    {

        return await _operationSettingQueryService.GetOperationSettings(request);
    }
}
