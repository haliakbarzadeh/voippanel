using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using MediatR;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetSoftPhoneEventsQuery : BaseQueryRequest, IRequest<PaginatedList<SoftPhoneEventDto>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public SoftPhoneEventType? EventType { get; set; }
    public List<long> UserIds { get; set; } = [];
}
