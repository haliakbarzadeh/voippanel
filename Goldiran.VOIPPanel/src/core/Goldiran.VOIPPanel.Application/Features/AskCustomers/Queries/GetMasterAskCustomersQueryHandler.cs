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

namespace Goldiran.VOIPPanel.Application.Features.AskCustomers.Queries;

public class GetMasterAskCustomersQueryHandler : IRequestHandler<GetMasterAskCustomersQuery, MasterAskCustomerDto>
{
    private readonly IAskCustomerQueryService _AskCustomerQueryService;
    private readonly IMapper _mapper;

    public GetMasterAskCustomersQueryHandler(IMapper mapper, IAskCustomerQueryService AskCustomerQueryService)
    {
        _mapper = mapper;
        _AskCustomerQueryService = AskCustomerQueryService;
    }

    public async Task<MasterAskCustomerDto> Handle(GetMasterAskCustomersQuery request, CancellationToken cancellationToken)
    {

        return await _AskCustomerQueryService.GetMasterAskCustomers(request);
    }
}
