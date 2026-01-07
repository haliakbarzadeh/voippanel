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

public class GetGeneralSecureCallQueryHandler : IRequestHandler<GetGeneralSecureCallQuery, GeneralSecureCallDto>
{
    private readonly ISecureCallQueryService _SecureCallQueryService;
    private readonly IMapper _mapper;

    public GetGeneralSecureCallQueryHandler(IMapper mapper, ISecureCallQueryService SecureCallQueryService)
    {
        _mapper = mapper;
        _SecureCallQueryService = SecureCallQueryService;
    }

    public async Task<GeneralSecureCallDto> Handle(GetGeneralSecureCallQuery request, CancellationToken cancellationToken)
    {

        return await _SecureCallQueryService.GetGeneralSecureCall(request);
    }
}
