using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.IQueries;

public interface ISoftPhoneEventQueryService
{
    public Task<PaginatedList<SoftPhoneEventDto>> GetAll(GetSoftPhoneEventsQuery request, CancellationToken cancellationToken);
}
