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

namespace Goldiran.VOIPPanel.Application.Features.Queus.Queries;


public class GetSeperatedQueusUsersQueryHandler : IRequestHandler<GetSeperatedQueusUsersQuery, QueusUsersResponse>
{
    private readonly IMapper _mapper;
    private readonly IQueuQueryService _queuQueryService;
    private readonly IQueueStatusService _queueStatusService;
    private readonly IUserPositionQueryService _userPositionQueryService;
    public GetSeperatedQueusUsersQueryHandler(IMapper mapper, IQueuQueryService queuQueryService, IQueueStatusService queueStatusService, IUserPositionQueryService userPositionQueryService)
    {
        _queuQueryService = queuQueryService;
        _queueStatusService = queueStatusService;
        _userPositionQueryService = userPositionQueryService;
        _mapper = mapper;
    }

    public async Task<QueusUsersResponse> Handle(GetSeperatedQueusUsersQuery request, CancellationToken cancellationToken)
    {
        return await _queuQueryService.GetSeperatedQueusUsers(request);
    }
}
