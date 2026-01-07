using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Azure.Core;
using Dapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Voip.Framework.Caching.Caching;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Voip.Framework.EFCore.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class QueueLogDetailsQueryService : IQueueLogDetailsQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly MySqlConnection _sqlConnection;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private int _retry = 0;

    public QueueLogDetailsQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IReadModelContext readModelContext, IMapper mapper, IDistributedCache cach)
    {
        _context = context;
        _askContext = askContext;
        _sqlConnection = sqlConnection;
        _readModelContext=readModelContext;
        _mapper = mapper;
        _cach = cach;
    }

    public async Task<QueueLogDetailsDto> GetQueueLogDetailss(GetDetailsQueueLogsQuery filter)
    {

        try
        {
            var eventList = new List<string>() { "ABANDON", "BLINDTRANSFER", "COMPLETEAGENT", "COMPLETECALLER", "EXITWITHTIMEOUT", "EXITWITHKEY", "EXITWITHEMPTY", "RINGCANCELED", "RINGNOANSWER" };
            var abondonEventList = new List<string>() { "ABANDON", "EXITWITHTIMEOUT", "EXITWITHKEY", "EXITWITHEMPTY" };

            var dto = new QueueLogDetailsDto();
            IList<string> queues = new List<string>();
            DateTime? fromDate = (filter.FromDate != null) ?((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0)):null;
            DateTime? toDate = (filter.ToDate != null ) ? ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)):null;

            if (filter.Queues.IsNullOrEmpty())
            {
                var up=await _readModelContext.Set<UserPosition>().Where(c=>c.IsActive && c.UserId==filter.UserId && c.PositionId==filter.PositionId).FirstOrDefaultAsync();
                queues=up.Queues.Split(",").ToList();
            }
            else
            {
                queues=filter.Queues.Select(q => q.ToString()).ToList();    
            }

            var amQuery = _askContext.AnsweringMachines.AsNoTracking().Where(c => (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                                             (filter.ToDate != null ? c.Date <= toDate : true) &&
                                                            (!queues.IsNullOrEmpty() ? queues.Contains(c.Queue) : true));

            var amCount = await amQuery.CountAsync();

            var query = _askContext.QueueLogs.AsNoTracking().Where(c => (filter.FromDate != null ? c.Created >= fromDate : true) &&
                                                                         (filter.ToDate != null ? c.Created <= toDate : true) &&
                                                                        (!queues.IsNullOrEmpty() ? queues.Contains(c.QueueNumber) : true));

            //dto.TotalCount=await query.Where(c=>eventList.Contains(c.Event)).Select(c=>c.CallId).Distinct().CountAsync();
            dto.TotalCount = await query.Where(c => eventList.Contains(c.Event)).Select(c => c.CallId).CountAsync(c => c != null);
            dto.AnsweredCount = await query.Where(c => c.Event== "COMPLETECALLER" || c.Event== "COMPLETEAGENT").Select(c => c.CallId).CountAsync(c => c != null);

            var abondonCount = await query.Where(c => c.Event== "ABANDON").CountAsync();
            dto.AbondonCount =abondonCount -amCount;


            if (dto.AnsweredCount+abondonCount == 0)
                dto.AbondonPercent = 0;
            else
                dto.AbondonPercent = (Math.Truncate((decimal)(dto.AbondonCount * 100) / (dto.AnsweredCount+ dto.AbondonCount)));

            dto.AnsweredPercent = 100 - dto.AbondonPercent;

            //if (dto.TotalCount == 0)
            //    dto.AnsweredPercent= 0;
            //else
            //    dto.AnsweredPercent = (Math.Truncate((decimal)(dto.AnsweredCount * 100) / dto.TotalCount));

            dto.CustomAnsweredCount = await query.Where(c => (c.Event == "COMPLETECALLER" || c.Event == "COMPLETEAGENT") && Convert.ToInt32(c.Data1)<=20).Select(c => c.CallId).CountAsync(c => c != null);


            if (dto.AnsweredCount + amCount == 0)
                dto.CustomAnsweredPercent = 0;
            else
                dto.CustomAnsweredPercent = (Math.Truncate((decimal)(dto.CustomAnsweredCount * 100) / (dto.AnsweredCount + amCount)));




            dto.WaitingAvgTime = (decimal)( await query.Where(c => (c.Event == "COMPLETECALLER" || c.Event == "COMPLETEAGENT")).Select(c =>Convert.ToInt32(c.Data1)).AverageAsync());
            dto.WaitingAvgTime = Math.Round(dto.WaitingAvgTime, 2);

            dto.AnsweringAvgTime = (decimal)(await query.Where(c => (c.Event == "COMPLETECALLER" || c.Event == "COMPLETEAGENT")).Select(c => Convert.ToInt32(c.Data2)).AverageAsync());
            dto.AnsweringAvgTime = Math.Round(dto.AnsweringAvgTime, 2);

            dto.GroupContactCounts = await GetGroupContactCount(query, eventList);
            dto.GroupSLA=await GetGroupSLA(query, new List<string>() { "COMPLETEAGENT", "COMPLETECALLER" });

            return dto;
        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetQueueLogDetailss(filter);
            }
            else
            {
                throw;

            }

        }

    }

    private async Task<List<QueueLogSLDto>> GetGroupContactCount(IQueryable<QueueLog> query, IList<string> eventList)
    {
        var results = await query.Where(c => eventList.Contains(c.Event)).GroupBy(c => c.Created.Hour).ToListAsync();

        return results.Select(c => new QueueLogSLDto() { Number = c.Key.ToString(), Count = c.Select(d => d.CallId).Count(c => c != null) }).ToList();

    }

    private async Task<List<QueueLogSLDto>> GetGroupSLA(IQueryable<QueueLog> query, IList<string> eventList)
    {
        var slNum = await query.Where(c => eventList.Contains(c.Event) && Convert.ToInt32(c.Data1)<=20).GroupBy(c => c.Created.Hour).ToListAsync();
        var total = await query.Where(c => eventList.Contains(c.Event)).GroupBy(c => c.Created.Hour).ToListAsync();

        var result= total.Select(c => new QueueLogSLDto() { Number = c.Key.ToString(), Count = c.Select(c=>c.CallId).Count(c => c != null) }).ToList();

        foreach (var item in result)
        {
            item.Count =(int)(Math.Truncate((decimal)((slNum.Where(c => c.Key.ToString() == item.Number).Select(c=>c.Count()).FirstOrDefault() * 100) / item.Count)));
        }

        return result;
    }


}
