using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using MediatR;
using System.Collections.Generic;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Voip.Framework.Common.Extensions;
using System.Reflection.Emit;
//using System.Reflection.Emit;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Queries;

public class GetRasadOperationQueryHandler : IRequestHandler<GetRasadOperationQuery, RasadOperationDto>
{
    private readonly IRasadOperationQueryService _rasadOperationQueryService;
    private readonly IOperationRasadService _operationRasadService;
    private readonly IMapper _mapper;

    public GetRasadOperationQueryHandler(IMapper mapper, IRasadOperationQueryService rasadOperationQueryService, IOperationRasadService operationRasadService)
    {
        _mapper = mapper;
        _rasadOperationQueryService = rasadOperationQueryService;
        _operationRasadService = operationRasadService;
    }

    public async Task<RasadOperationDto> Handle(GetRasadOperationQuery request, CancellationToken cancellationToken)
    {
        var result= await _rasadOperationQueryService.GetRasadOperation(request);

        if (result.RasadUserOperationList.IsNullOrEmpty()) 
        {
            return result;
        }

        var operationRasadResponseList =await _operationRasadService.GetOperationRasad(new OperationRasadRequest() { IP= "10.14.14.34" });
        operationRasadResponseList.AddRange( await _operationRasadService.GetOperationRasad(new OperationRasadRequest() { IP = "10.14.14.11" }));

        //var operationRasadResponseList = new List<OperationRasadResponse>();

        foreach (var item in result.RasadUserOperationList)
        {
            var operationRasadResponse= operationRasadResponseList.Where(x => x.Extension == item.Extension).FirstOrDefault();
            if (operationRasadResponse != null && !string.IsNullOrEmpty( operationRasadResponse.QueueCode) && operationRasadResponse.QueueCode!="s" && item.OperationTypeId!= Goldiran.VOIPPanel.Domain.AggregatesModel.Files.OperationType.Exit)
            {
                item.Queue =operationRasadResponse.QueueCode;
                item.IsAnswering = true;
                item.AnsweringDuration=operationRasadResponse.Duration;
                item.Caller=operationRasadResponse.Caller;
            }
            else
            {
                item.Queue = "-";
                item.IsAnswering = false;
                item.AnsweringDuration = "-";
                item.Caller = "-";
            }

        }

        return result;  

    }


}
