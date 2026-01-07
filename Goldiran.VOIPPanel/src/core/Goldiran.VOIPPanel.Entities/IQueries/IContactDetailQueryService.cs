using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;

public interface IContactDetailQueryService : IBaseQueryService
{
    public Task<PaginatedList<ContactDetailDto>> GetContactDetails(GetContactDetailsQuery filter);
    public Task<PaginatedList<AutoDialDto>> GetAutoDetails(GetAutoDetailsQuery filter);
    public Task<PaginatedList<ContactDetailDto>> GetFlatContactDetails(GetContactDetailsQuery filter);
    public Task<PaginatedList<AutoDialDto>> GetFlatAutoDetails(GetAutoDetailsQuery filter);

    //public Task<PaginatedList<ContactDetailDto>> GetGroupedContactDetail(GetGroupedContactDetailQuery filter);
    //public Task<PaginatedList<ContactDetailDto>> GetContactDetailsByLinkedId(GetContactDetailsByLinkedIdQuery filter);
}
