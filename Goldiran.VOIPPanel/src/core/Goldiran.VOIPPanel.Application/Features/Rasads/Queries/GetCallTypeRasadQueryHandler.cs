using AutoMapper;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Common.Extensions;
using Voip.Framework.EFCore;
using ReadModelEntity=Goldiran.VOIPPanel.ReadModel.Entities;
//using System.Reflection.Emit;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Queries;

public class GetCallTypeRasadQueryHandler : IRequestHandler<GetRasadCallTypeQuery, RasadCallTypeDto>
{
    private readonly IReadModelContext _readModelContext;
    private readonly IOperationRasadService _operationRasadService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCallTypeRasadQueryHandler(IMapper mapper, IReadModelContext readModelContext, IOperationRasadService operationRasadService, ILogger<GetCallTypeRasadQueryHandler> logger)
    {
        _mapper = mapper;
        _readModelContext = readModelContext;
        _operationRasadService = operationRasadService;
        _logger = logger;
    }

    public async Task<RasadCallTypeDto> Handle(GetRasadCallTypeQuery request, CancellationToken cancellationToken)
    {
        var dto = new RasadCallTypeDto();

        var operationRasadResponseList =await _operationRasadService.GetOperationRasad(new OperationRasadRequest() { IP= "10.14.14.34" });

        foreach (var item in operationRasadResponseList)
        {
            var operationRasadResponse= operationRasadResponseList.Where(x => x.Extension == item.Extension).FirstOrDefault();
            if (operationRasadResponse != null && !string.IsNullOrEmpty( operationRasadResponse.QueueCode) && operationRasadResponse.QueueCode!="s" && request.Agent==item.Extension )
            {
                dto.CallerId = operationRasadResponse.Caller;
                break;
            }

        }
        if (string.IsNullOrEmpty(dto.CallerId)) 
        {
            return dto;
        }

        DateTime toDate = DateTime.Now;
        DateTime fromDate = toDate.AddDays(-2);

        var callCount = await _readModelContext.Set<ReadModelEntity.ContactDetail>().Where(c => EF.Functions.Like(c.Source, $"%{dto.CallerId}%") && c.ReportType == ReportType.Normal && c.Date >= fromDate && c.Date <= toDate).CountAsync();
        
        dto.CallCount = callCount;
        dto.CallerId = GetNormalization(dto.CallerId);

        if (string.IsNullOrEmpty(dto.CallerId))
        {
            _logger.LogInformation($"log for calltype Agent {request.Agent}");
        }

        
        return dto;  

    }

    private string GetNormalization(string callerId)
    {
        string result=callerId;
        if (callerId.Substring(0, 3) == "098")
        {
            result = $"0{callerId.Substring(3,callerId.Length-3)}";
        }
        else if (callerId.Substring(0, 4) == "0098")
        {
            result = $"0{callerId.Substring(4, callerId.Length - 4)}";
        }
        else if (callerId.Substring(0, 3) == "+98")
        {
            result = $"0{callerId.Substring(3, callerId.Length - 3)}";
        }
        else if (callerId.Substring(0,2 ) == "98")
        {
            result = $"0{callerId.Substring(2, callerId.Length - 2)}";
        }
        else if (callerId.Length==8 && callerId[0]!='0')
        {
            result = $"021{callerId}";
        }
        else if (callerId[0] != '0')
        {
            result = $"0{callerId}";
        }

        return result.Trim();
    }


}
