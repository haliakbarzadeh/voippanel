using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Azure.Core;
using MediatR;
using Voip.Framework.Common.Extensions;
using Goldiran.VOIPPanel.ReadModel.Entities;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class AskCustomerQueryService : IAskCustomerQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public AskCustomerQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _askContext = askContext;
        _mapper = mapper;
    }


    public async Task<PaginatedList<AskCustomerDto>> GetAskCustomers(GetAskCustomersQuery filter)
    {
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;

        List<UserPosition> positions = new List<UserPosition>();
        if (string.IsNullOrEmpty(filter.Agents))
        {
            positions = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (!filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).ToListAsync();
            var agents = positions.Select(c => c.Extension).ToList();
            filter.Agents = string.Join(",", agents);
        }
        else
        {
            positions = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                .Where(c => (c.IsActive) &&
                (!filter.HasContentAccess ? filter.Agents.Contains(c.Extension) && (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).ToListAsync();

        }

        var agentList = filter.Agents.Split(",").Select(c => Convert.ToInt32(c)).ToList();
        var scoreList = filter.Scorces.Select(c => c.ToString()).ToList();
        string queue = filter.Code != null ? filter.Code.ToString() : string.Empty;

        var query = from cust in _askContext.AskCustomers
                    orderby cust.Id descending
                    where (fromDate != null ? cust.Date >= fromDate : true) &&
                          (toDate != null ? cust.Date <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? agentList.Contains(cust.Agent) : true) &&
                          (filter.Code != null ? cust.Queue == queue : true) &&
                          (!filter.Scorces.IsNullOrEmpty() ? scoreList.Contains(cust.Response) : true)
                    select new { cust };



        var count = query.Count();

        var entities = await query
        .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToListAsync();

        var results = from cust in entities
                      join up in positions on cust.cust.Agent.ToString() equals up.Extension
                      orderby cust.cust.Id descending
                      where (fromDate != null ? cust.cust.Date >= fromDate : true) &&
                            (toDate != null ? cust.cust.Date <= toDate : true) &&
                            (!agentList.IsNullOrEmpty() ? agentList.Contains(cust.cust.Agent) : true)
                      select new { cust, up };

        var dtos = new List<AskCustomerDto>();

        foreach (var entity in results)
        {
            var dto = new AskCustomerDto()
            {
                Id = entity.cust.cust.Id,
                CallerId = entity.cust.cust.CallerId,
                Agent = entity.cust.cust.Agent,
                Date = entity.cust.cust.Date,
                Queue = entity.cust.cust.Queue,
                Response = entity.cust.cust.Response,
                Time = entity.cust.cust.Time,
                UniqueId = entity.cust.cust.UniqueId,
                UserName = entity.up.User.UserName,
                Guid = entity.up.User.Guid

            };

            dtos.Add(dto);
        }
        return new PaginatedList<AskCustomerDto>(dtos, count, filter.PageNumber, filter.PageSize);

    }

    public async Task<MasterAskCustomerDto> GetMasterAskCustomers(GetMasterAskCustomersQuery filter)
    {
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;

        List<UserPosition> positions = new List<UserPosition>();
        if (string.IsNullOrEmpty(filter.Agents))
        {
            positions = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (!filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).ToListAsync();
            var agents = positions.Select(c => c.Extension).ToList();
            filter.Agents = string.Join(",", agents);
        }
        else
        {
            positions = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                .Where(c => (c.IsActive) &&
                (!filter.HasContentAccess ? filter.Agents.Contains(c.Extension) && (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).ToListAsync();

        }

        var agentList = filter.Agents.Split(",").Select(c => Convert.ToInt32(c)).ToList();
        string queue = filter.Code != null ? filter.Code.ToString() : string.Empty;
        var scorcesList = filter.Scorces.Select(c => c.ToString()).ToList();


        var dto = new MasterAskCustomerDto();
        dto.ContributorPercent =await GetContributeValue(fromDate, toDate, positions, agentList, queue,scorcesList);
        dto.CSATNumbers = await GetCSATNumbers(fromDate, toDate, positions, agentList, queue);
        dto.GroupContributors = await GetGroupContributors(fromDate, toDate, positions, agentList, queue);
        dto.GroupCSATNumbers = await GetGroupCSATNumbers(fromDate, toDate, positions, agentList, queue);
        
        
        var CSATTotal= dto.CSATNumbers.Select(c=>c.Count).Sum();
        var CSAT = dto.CSATNumbers.Where(c=>c.Number=="4" || c.Number == "5").Select(c => c.Count).Sum();

        if (CSATTotal == 0)
        {
            dto.CSAT = null;
        }
        else
        {
            dto.CSAT = (CSAT*100)/CSATTotal;

        }

        return dto;
    }

    private async Task<decimal> GetContributeValue(DateTime? fromDate, DateTime? toDate, IList<UserPosition> positions, IList<int> agentList, string queue, IList<string> scorces)    {
        List<string> eventList = new List<string>() { "COMPLETEAGENT", "COMPLETECALLER" };

        var contributeQuery = from cust in _askContext.AskCustomers
                    orderby cust.Id descending
                    where (fromDate != null ? cust.Date >= fromDate : true) &&
                          (toDate != null ? cust.Date <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? agentList.Contains(cust.Agent) : true) &&
                          (!scorces.IsNullOrEmpty() ? scorces.Contains(cust.Response) : true) &&
                          (!string.IsNullOrEmpty(queue) ? cust.Queue == queue : true)
                    select new { cust };

        var contribute= contributeQuery.Count();

        var tempAgents= agentList.Select(c=>c.ToString()).ToList();
        var tempSIPAgents = agentList.Select(c =>$"SIP/{c.ToString()}").ToList();

        var totalQuery = await _askContext.QueueLogs.AsNoTracking().Where(c => eventList.Contains(c.Event) &&
                          (fromDate != null ? c.Created >= fromDate : true) &&
                          (toDate != null ? c.Created <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? (tempAgents.Contains(c.Agent) || tempSIPAgents.Contains(c.Agent)) : true) &&
                          (!string.IsNullOrEmpty(queue) ? c.QueueNumber == queue : true)).GroupBy(c => c.CallId).CountAsync();

        if (totalQuery == 0)
            return 0;
        else
            return Math.Truncate( (decimal)((contribute*100)/totalQuery));
    }

    private async Task<List<NumberAskDto>> GetCSATNumbers(DateTime? fromDate, DateTime? toDate, IList<UserPosition> positions, IList<int> agentList, string queue)
    {
        var results = await _askContext.AskCustomers.AsNoTracking().Where(c => (fromDate != null ? c.Date >= fromDate : true) &&
                          (toDate != null ? c.Date <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? agentList.Contains(c.Agent) : true) &&
                          (!string.IsNullOrEmpty(queue) ? c.Queue == queue : true)).GroupBy(c => c.Response).ToListAsync();

        return results.Select(c => new NumberAskDto() { Number = c.Key, Count = c.Count() }).OrderBy(c=>c.Count).ToList();

    }

    private async Task<List<GroupAskDto>> GetGroupContributors(DateTime? fromDate, DateTime? toDate, IList<UserPosition> positions, IList<int> agentList, string queue)
    {
        var results = await _askContext.AskCustomers.AsNoTracking().Where(c => (fromDate != null ? c.Date >= fromDate : true) &&
                          (toDate != null ? c.Date <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? agentList.Contains(c.Agent) : true) &&
                          (!string.IsNullOrEmpty(queue) ? c.Queue == queue : true)).GroupBy(c => c.Time.Hours).ToListAsync();

        return results.Select(c => new GroupAskDto() { RowValue = c.Key.ToString(), ColumnValue = c.Count() }).ToList();

    }

    private async Task<List<GroupAskDto>> GetGroupCSATNumbers(DateTime? fromDate, DateTime? toDate, IList<UserPosition> positions, IList<int> agentList, string queue)
    {
        var results = await _askContext.AskCustomers.AsNoTracking().Where(c => (fromDate != null ? c.Date >= fromDate : true) &&
                          (toDate != null ? c.Date <= toDate : true) &&
                          (!agentList.IsNullOrEmpty() ? agentList.Contains(c.Agent) : true) &&
                          (!string.IsNullOrEmpty(queue) ? c.Queue == queue : true)).GroupBy(c => c.Time.Hours).ToListAsync();

        return results.Select(c => new GroupAskDto() { RowValue = c.Key.ToString(), ColumnValue =Math.Truncate( c.Select(d=>Convert.ToDecimal(d.Response)).Average()*20) }).ToList();

    }
}

