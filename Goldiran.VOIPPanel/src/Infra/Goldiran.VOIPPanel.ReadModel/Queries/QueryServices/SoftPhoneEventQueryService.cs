using AutoMapper;
using AutoMapper.QueryableExtensions;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.IQueries;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class SoftPhoneEventQueryService : ISoftPhoneEventQueryService
{
    private readonly IMapper _mapper;
    private readonly VOIPPanelReadModelContext _context;

    public SoftPhoneEventQueryService(IMapper mapper, VOIPPanelReadModelContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<SoftPhoneEventDto>> GetAll(GetSoftPhoneEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.FromDate.HasValue)
        {
            var date = DateOnly.FromDateTime(request.FromDate.Value);
            request.FromDate = new DateTime(date, TimeOnly.MinValue);
        }

        if (request.ToDate.HasValue)
        {
            var date = DateOnly.FromDateTime(request.ToDate.Value);
            request.ToDate = new DateTime(date, TimeOnly.MaxValue);
        }


        var result = await _context.SoftPhoneEvents
            .AsNoTracking()
            .Where(x => (request.FromDate.HasValue ? (x.StartedAt >= request.FromDate) : true) &&
            (request.ToDate.HasValue ? (x.StartedAt <= request.ToDate) : true) &&
            (request.UserIds.Count > 0 ? (request.UserIds.Contains(x.UserId)) : true) &&
            (request.EventType.HasValue ? x.EventType == request.EventType : true) &&
            (!request.HasContentAccess ? (x.UserId == request.UserId && x.UserPosition.PositionId == request.PositionId) || request.PositionCildIds.Contains(x.UserPosition.PositionId) : true)
            )
            .OrderByDescending(x => x.Id)
            .ProjectTo<SoftPhoneEventDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return result;
    }
}
