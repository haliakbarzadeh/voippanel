using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.IQueries;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.SoftPhoneEvents.Queries;

public class GetSoftPhoneEventsQueryHandler : IRequestHandler<GetSoftPhoneEventsQuery, PaginatedList<SoftPhoneEventDto>>
{
    private readonly ISoftPhoneEventQueryService _softPhoneEventQueryService;

    public GetSoftPhoneEventsQueryHandler(ISoftPhoneEventQueryService softPhoneEventQueryService)
    {
        _softPhoneEventQueryService = softPhoneEventQueryService;
    }

    public Task<PaginatedList<SoftPhoneEventDto>> Handle(GetSoftPhoneEventsQuery request, CancellationToken cancellationToken)
    {
        var result = _softPhoneEventQueryService.GetAll(request, cancellationToken);

        return result;
    }
}
