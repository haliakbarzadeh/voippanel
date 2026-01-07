using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Mappers;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class QueueLimitationQueryService : IQueueLimitationQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public QueueLimitationQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<QueueLimitationDto> GetQueueLimitationById(int id)
    {
        var entity=await _context.QueueLimitations.Include(c=>c.HourValues).AsNoTracking().FirstOrDefaultAsync(c=>c.QueueId==id);
        return _mapper.Map<QueueLimitationDto>(entity);
    }

    public async Task<PaginatedList<QueueLimitationDto>> GetQueueLimitations(GetQueueLimitationsQuery filter)
    {
        var result = await _context.QueueLimitations.Include(c => c.HourValues) .AsNoTracking()
                        .Where(c =>filter.QueueId!=null ? c.QueueId == filter.QueueId: true)
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<QueueLimitationDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }


}
