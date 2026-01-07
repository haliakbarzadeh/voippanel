using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.AnsweringMachines.Queries;

public class GetAnsweringMachinesQueryHandler : IRequestHandler<GetAnsweringMachinesQuery, PaginatedList<AnsweringMachineDto>>
{
    private readonly IAnsweringMachineQueryService _answeringMachineQueryService;
    private readonly IMapper _mapper;

    public GetAnsweringMachinesQueryHandler(IMapper mapper, IAnsweringMachineQueryService answeringMachineQueryService)
    {
        _mapper = mapper;
        _answeringMachineQueryService = answeringMachineQueryService;
    }

    public async Task<PaginatedList<AnsweringMachineDto>> Handle(GetAnsweringMachinesQuery request, CancellationToken cancellationToken)
    {

        if (request.FromDate == null || request.ToDate == null)
        {
            throw new ValidationException(new List<string>() { "بازه تاریخی را مشخص کنید" });
        }

        return await _answeringMachineQueryService.GetAnsweringMachines(request);

    }
}
