using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Queries;


public class GetQueusUsersQueryHandler : IRequestHandler<GetQueusUsersQuery, PaginatedList<QueuUserDto>>
{
    private readonly IMapper _mapper;
    private readonly IQueuQueryService _queuQueryService;
    private readonly IQueueStatusService _queueStatusService;
    private readonly IUserPositionQueryService _userPositionQueryService;
    public GetQueusUsersQueryHandler(IMapper mapper, IQueuQueryService queuQueryService, IQueueStatusService queueStatusService, IUserPositionQueryService userPositionQueryService)
    {
        _queuQueryService = queuQueryService;
        _queueStatusService = queueStatusService;
        _userPositionQueryService = userPositionQueryService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QueuUserDto>> Handle(GetQueusUsersQuery request, CancellationToken cancellationToken)
    {
        var userPosition = await _userPositionQueryService.GetUserPositionById((long)request.PositionId);
        var extensions =await _queueStatusService.GetQueueStatus(new QueueStatusRequest() { QueueCodeList = userPosition.Queues.Split(',').ToList()});

        var extensionList=new List<string>();
        foreach (var item in extensions)
        {
            extensionList.AddRange(item.ExtensionList.Where(c => !extensionList.Any(c => extensionList.Contains(c))).ToList());
        }

        request.Extensions = extensionList;
        request.QueueList = userPosition.Queues.Split(',').ToList();
        //request.Extensions = extensions[0].ExtensionList;
        return await _queuQueryService.GetQueusUsers(request);
    }
}
