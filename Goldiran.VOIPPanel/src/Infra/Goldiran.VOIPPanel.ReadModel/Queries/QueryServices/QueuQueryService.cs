using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class QueuQueryService : IQueuQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public QueuQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QueuDto>> GetQueus(GetQueusQuery filter)
    {
        var result = await _context.Queu.AsNoTracking()
                        .OrderByDescending(x => x.Created)
                        .QueryEntityResult(filter)
                        .ProjectTo<QueuDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize, true);

        return result;
    }

    public async Task<PaginatedList<QueuDto>> GetPositionQueues(GetPositionQueusQuery filter)
    {
        var positionQueues = await _context.UserPosition.AsNoTracking().Where(c => c.IsActive && c.PositionId == filter.PositionId).FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(positionQueues.Queues))
        {
            return new PaginatedList<QueuDto>(null, 0, filter.PageNumber, filter.PageSize);
        }
        var queueList = positionQueues.Queues.Split(',');

        var result = await _context.Queu.AsNoTracking()
                        .Where(c => filter.HasContentAccess || queueList.Contains(c.Code.ToString()))
                        .OrderByDescending(x => x.Created)
                        .QueryEntityResult(filter)
                        .ProjectTo<QueuDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize, filter.DisablePaging);

        return result;
    }

    public async Task<PaginatedList<QueuUserDto>> GetQueusUsers(GetQueusUsersQuery filter)
    {
        string queueCode = filter.QueueName.Split('_')[0];
        //var queue = await _context.Queu.AsNoTracking().FirstOrDefaultAsync(c => c.Id == filter.QueueId);
        var result = await _context.User.AsNoTracking().Include(c => c.UserPositions).ThenInclude(c => c.Position)
                                    .Where(c => c.UserPositions.Any(d => filter.Extensions.Contains(d.Extension) && d.Position.ContactedParentPositionId.Contains($"'{filter.PositionId}'")))
                         //.Where(c=>c.UserPositions.Any(d=> filter.Extensions.Contains(d.Extension) && EF.Functions.Like(d.Position.ContactedParentPositionId,$"%'{filter.PositionId}'%") && EF.Functions.Like(d.Queues, $"%{queueCode}%")))
                         .OrderByDescending(x => x.Created)
                        .QueryEntityResult(filter)
                        .ProjectTo<QueuUserDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize, true);
        return result;
    }

    public async Task<QueusUsersResponse> GetSeperatedQueusUsers(GetSeperatedQueusUsersQuery filter)
    {
        string queueCode = filter.QueueName.Split('_')[0];

        var result = await _context.UserPosition.AsNoTracking().Include(c => c.Position).Include(c => c.User)
                                    .Where(c => c.User.IsActive && c.IsActive && c.Position.ContactedParentPositionId.Contains($"'{filter.PositionId}'")).ToListAsync();

        var addedUser = result.Where(c => c.IsActive && c.Queues.Contains(queueCode) && c.Position.ContactedParentPositionId.Contains($"'{filter.PositionId}'"));

        var addedUserDto = _mapper.Map<List<QueuUserDto>>(addedUser);

        var removedUser = result.Where(c => c.IsActive && !c.Queues.Contains(queueCode) && c.Position.ContactedParentPositionId.Contains($"'{filter.PositionId}'"));

        var removedUserDto = _mapper.Map<List<QueuUserDto>>(removedUser);

        return new QueusUsersResponse() { AddedUsers = addedUserDto, RemovedUsers = removedUserDto };

    }

}
