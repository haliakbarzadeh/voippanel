using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;

namespace Goldiran.VOIPPanel.Application.Features.AskCustomers.Queries;

public class GetWallboardReportQueryHandler : IRequestHandler<GetWallboardsQuery, WallboardReportDto>
{
    private readonly IWallboardReportQueryService _wallboardReportQueryService;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;

    public GetWallboardReportQueryHandler(IMapper mapper, IWallboardReportQueryService wallboardReportQueryService, IReadModelContext readModelContext)
    {
        _mapper = mapper;
        _readModelContext = readModelContext;
        _wallboardReportQueryService = wallboardReportQueryService;
    }

    public async Task<WallboardReportDto> Handle(GetWallboardsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == 1)
        {
            request.PositionId = 11;
        }
        var position =  _readModelContext.Set<Position>().AsNoTracking().FirstOrDefault(c => c.Id == request.PositionId);

        var positionIds = new List<long>();
        if (Convert.ToBoolean(position.HasShifts))
        {
            if (DateTime.Now.TimeOfDay.TotalSeconds > 57600)
            {
                positionIds = _readModelContext.Set<Position>().AsNoTracking().Where(c => c.ShiftType == ShiftType.Night && EF.Functions.Like(c.ContactedParentPositionId, $"%'{request.PositionId}'%")).Select(c => c.Id).ToList();
                request.IsMorningShift = false;

            }
            else
            {
                positionIds = _readModelContext.Set<Position>().AsNoTracking().Where(c => (c.ShiftType == ShiftType.Morning) && EF.Functions.Like(c.ContactedParentPositionId, $"%'{request.PositionId}'%")).Select(c => c.Id).ToList();
                request.IsMorningShift = true;

            }

        }
        else
        {
            positionIds = _readModelContext.Set<Position>().AsNoTracking().Where(c => EF.Functions.Like(c.ContactedParentPositionId, $"%'{request.PositionId}'%")).Select(c => c.Id).ToList();

        }

        request.PositionIds = positionIds;

        switch (request.wallboardReportType)
        {
            case WallboardReportType.AgentContacts:
                return await _wallboardReportQueryService.GetAgentContactsCount(request);
            case WallboardReportType.AverageTimeContact:
                return await _wallboardReportQueryService.GetCallAverageTime(request);
            case WallboardReportType.SumTimeContact:
                return await _wallboardReportQueryService.GetCallSumTime(request);
                case WallboardReportType.CSAT:
                return await _wallboardReportQueryService.GetCSAT(request);
            case WallboardReportType.RestOperation:
            case WallboardReportType.AnsweringOperation:
                return await _wallboardReportQueryService.GetOperationStatus(request);
             default:
                return null;
        }
        //return await _wallboardReportQueryService.GetCallSumTime(request);

    }


}
