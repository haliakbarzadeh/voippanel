//using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
//using Goldiran.VOIPPanel.ReadModel.Dto;
//using Goldiran.VOIPPanel.ReadModel.QueryRequest;
//using MediatR;
//using Voip.Framework.Common.Exceptions;
//using Voip.Framework.Domain.Models.CQRS;

//namespace Goldiran.VOIPPanel.Application.Features.ContactDetails.Queries
//{
//    public class GetGroupedContactDetailQueryHandler : IRequestHandler<GetGroupedContactDetailQuery, PaginatedList<ContactDetailDto>>
//    {
//        private readonly IContactDetailQueryService _contactDetailQueryService;

//        public GetGroupedContactDetailQueryHandler(IContactDetailQueryService contactDetailQueryService)
//        {
//            _contactDetailQueryService = contactDetailQueryService;
//        }

//        public async Task<PaginatedList<ContactDetailDto>> Handle(GetGroupedContactDetailQuery request, CancellationToken cancellationToken)
//        {
//            if (request.FromDate == null || request.ToDate == null)
//            {
//                throw new ValidationException(new List<string>() { "بازه تاریخی را مشخص کنید" });
//            }

//            //return await _contactDetailQueryService.GetContactDetails(request);


//            DateTime tempDate = new DateTime(2025, 3, 21, 0, 0, 0);

//            if (request.FromDate < tempDate || request.ToDate < tempDate)
//                throw new ValidationException(new List<string>() { "" });

//            return await _contactDetailQueryService.GetGroupedContactDetail(request);
//        }
//    }
//}
