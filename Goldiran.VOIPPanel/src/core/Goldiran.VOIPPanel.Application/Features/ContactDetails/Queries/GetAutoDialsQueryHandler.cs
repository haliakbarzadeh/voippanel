using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.ContactDetails.Queries;

public class GetAutoDialssQueryHandler : IRequestHandler<GetAutoDetailsQuery, PaginatedList<AutoDialDto>>
{
    private readonly IContactDetailQueryService _AutoDialsQueryService;
    private readonly IMapper _mapper;

    public GetAutoDialssQueryHandler(IMapper mapper, IContactDetailQueryService AutoDialsQueryService)
    {
        _mapper = mapper;
        _AutoDialsQueryService = AutoDialsQueryService;
    }

    public async Task<PaginatedList<AutoDialDto>> Handle(GetAutoDetailsQuery request, CancellationToken cancellationToken)
    {

        if (request.OperationReportType == null && (request.FromDate == null || request.ToDate == null))
        {
            throw new ValidationException(new List<string>() { "بازه تاریخی را مشخص کنید" });
        }

        //return await _AutoDialsQueryService.GetAutoDialss(request);


        return await _AutoDialsQueryService.GetFlatAutoDetails(request);

    }
}
