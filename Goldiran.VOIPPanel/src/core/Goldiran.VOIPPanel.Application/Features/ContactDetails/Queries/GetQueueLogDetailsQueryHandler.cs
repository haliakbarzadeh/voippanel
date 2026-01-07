using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.ContactDetails.Queries;

public class GetQueueLogDetailsQueryHandler : IRequestHandler<GetQueueLogsQuery, PaginatedList<QueueLogDto>>
{
    private readonly IQueueLogQueryService _queueLogQueryService;
    private readonly IMapper _mapper;

    public GetQueueLogDetailsQueryHandler(IMapper mapper, IQueueLogQueryService queueLogQueryService)
    {
        _mapper = mapper;
        _queueLogQueryService = queueLogQueryService;
    }

    public async Task<PaginatedList<QueueLogDto>> Handle(GetQueueLogsQuery request, CancellationToken cancellationToken)
    {

        if (request.FromDate == null || request.ToDate == null)
        {
            throw new ValidationException(new List<string>() { "بازه تاریخی را مشخص کنید" });
        }

        return await _queueLogQueryService.GetQueueLogs(request);

    }
}
