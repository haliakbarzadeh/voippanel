using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.SecureCalls.Queries;

public class GetSecureCallsQueryHandler : IRequestHandler<GetSecureCallsQuery, PaginatedList<SecureCallDto>>
{
    private readonly ISecureCallQueryService _SecureCallQueryService;
    private readonly IMapper _mapper;

    public GetSecureCallsQueryHandler(IMapper mapper, ISecureCallQueryService SecureCallQueryService)
    {
        _mapper = mapper;
        _SecureCallQueryService = SecureCallQueryService;
    }

    public async Task<PaginatedList<SecureCallDto>> Handle(GetSecureCallsQuery request, CancellationToken cancellationToken)
    {

        return await _SecureCallQueryService.GetSecureCalls(request);
    }
}
