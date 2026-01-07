//using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
//using Goldiran.VOIPPanel.ReadModel.Dto;
//using Goldiran.VOIPPanel.ReadModel.QueryRequest;
//using MediatR;
//using Voip.Framework.Domain.Models.CQRS;

//namespace Goldiran.VOIPPanel.Application.Features.ContactDetails.Queries
//{
//    public class GetContactDetailsByLinkedIdQueryHandler : IRequestHandler<GetContactDetailsByLinkedIdQuery, PaginatedList<ContactDetailDto>>
//    {
//        private readonly IContactDetailQueryService _contactDetailQueryService;

//        public GetContactDetailsByLinkedIdQueryHandler(IContactDetailQueryService contactDetailQueryService)
//        {
//            _contactDetailQueryService = contactDetailQueryService;
//        }
//        public async Task<PaginatedList<ContactDetailDto>> Handle(GetContactDetailsByLinkedIdQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _contactDetailQueryService.GetContactDetailsByLinkedId(request);

//            return result;
//        }
//    }
//}
