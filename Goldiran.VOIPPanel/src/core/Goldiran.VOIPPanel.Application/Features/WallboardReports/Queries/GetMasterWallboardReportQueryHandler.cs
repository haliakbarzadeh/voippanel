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

public class GetMasterWallboardReportQueryHandler : IRequestHandler<GetMasterWallboardQuery, MasterWallboardReportDto>
{
    private readonly IMasterWallboardReportQueryService _wallboardReportQueryService;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;

    public GetMasterWallboardReportQueryHandler(IMapper mapper, IMasterWallboardReportQueryService wallboardReportQueryService, IReadModelContext readModelContext)
    {
        _mapper = mapper;
        _readModelContext = readModelContext;
        _wallboardReportQueryService = wallboardReportQueryService;
    }

    public async Task<MasterWallboardReportDto> Handle(GetMasterWallboardQuery request, CancellationToken cancellationToken)
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

        return await _wallboardReportQueryService.GetMasterWallboardData(request);

    }


}
