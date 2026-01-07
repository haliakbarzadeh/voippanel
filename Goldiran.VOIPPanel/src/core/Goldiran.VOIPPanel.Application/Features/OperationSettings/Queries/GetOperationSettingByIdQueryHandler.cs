using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Goldiran.VOIPPanel.Application.Features.OperationSettings.Queries;

public class GetOperationSettingByIdQueryHandler : IRequestHandler<GetOperationSettingByIdQuery, OperationSettingDto>
{
    private readonly IOperationSettingQueryService _operationSettingQueryService; 
    public GetOperationSettingByIdQueryHandler(IOperationSettingQueryService operationSettingQueryService)
    {
        _operationSettingQueryService = operationSettingQueryService;
    }

    public async Task<OperationSettingDto> Handle(GetOperationSettingByIdQuery request, CancellationToken cancellationToken)
    {

        return await _operationSettingQueryService.GetOperationSettingById(request.Id);
    }
}
