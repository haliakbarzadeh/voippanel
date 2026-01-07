using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest
{
    public class GetContactDetailsByLinkedIdQuery : BaseQueryRequest, IRequest<PaginatedList<ContactDetailDto>>
    {
        public string LinkedId { get; set; }
    }
}
