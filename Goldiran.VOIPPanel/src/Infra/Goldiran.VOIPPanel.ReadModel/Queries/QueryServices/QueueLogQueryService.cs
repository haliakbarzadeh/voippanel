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
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Voip.Framework.EFCore.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class QueueLogQueryService : IQueueLogQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly MySqlConnection _sqlConnection;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private int _retry = 0;

    public QueueLogQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IReadModelContext readModelContext, IMapper mapper, IDistributedCache cach)
    {
        _context = context;
        _askContext = askContext;
        _sqlConnection = sqlConnection;
        _readModelContext=readModelContext;
        _mapper = mapper;
        _cach = cach;
    }

    public async Task<PaginatedList<QueueLogDto>> GetQueueLogs(GetQueueLogsQuery filter)
    {

        try
        {

            var eventList=new List<string>() { "ABANDON" , "BLINDTRANSFER" , "COMPLETEAGENT" , "COMPLETECALLER", "EXITWITHTIMEOUT" , "EXITWITHKEY", "RINGCANCELED", "RINGNOANSWER" };   
            IList<string> agents = new List<string>();
            IList<string> queues = new List<string>();
            var fromDate = (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0));
            var toDate = (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59));
                        

            if ( !filter.Agents.IsNullOrEmpty())
            {
                agents= filter.Agents.Select(c=>$"SIP/{c.ToString()}").ToList();
            }

            if (filter.Queues.IsNullOrEmpty())
            {
                var up=await _readModelContext.Set<UserPosition>().Where(c=>c.IsActive && c.UserId==filter.UserId && c.PositionId==filter.PositionId).FirstOrDefaultAsync();
                queues=up.Queues.Split(",").ToList();
            }
            else
            {
                queues=filter.Queues.Select(q => q.ToString()).ToList();    
            }
            if (!filter.QueueLogEvents.IsNullOrEmpty())
            {
                eventList=filter.QueueLogEvents.Select(q => q.ToString()).ToList(); 
            }

            var query =from log in _askContext.QueueLogs 
                         join sublog in _askContext.QueueLogs on log.CallId equals sublog.CallId
                         where (!agents.IsNullOrEmpty() ? agents.Contains(log.Agent)  : true) &&
                                                                         (!eventList.IsNullOrEmpty() ? eventList.Contains(log.Event) : true) &&
                                                                         (filter.FromDate != null ? log.Created >= fromDate : true) &&
                                                                         (filter.ToDate != null ? log.Created <= toDate : true) &&
                                                                        (!queues.IsNullOrEmpty() ? queues.Contains(log.QueueNumber) : true) &&
                                                                        (!string.IsNullOrEmpty(filter.Phone) ? sublog.Data2.Contains(filter.Phone): true) &&
                                                                        sublog.Event== "ENTERQUEUE" && sublog.QueueNumber==log.QueueNumber
                       select new { log, sublog};

            var count = query.Count();

            var entities = await query.OrderByDescending(c=>c.log.Created)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToListAsync();

            var dtos = new List<QueueLogDto>();

            foreach (var entity in entities)
            {
                var dto = new QueueLogDto()
                {
                    Id = entity.log.Id,
                    CallId= entity.sublog.Data2,
                    Data1 = entity.log.Data1,
                    Data2 = entity.log.Data2,
                    Data3 = entity.log.Data3,
                    Data4 = entity.log.Data4,
                    Data5 = entity.log.Data5,
                    Created = entity.log.Created,
                    Time=entity.log.Time,
                    Agent=entity.log.Agent, 
                    Event=entity.log.Event,
                    QueueNumber=entity.log.QueueNumber,
                    

                };

                dtos.Add(dto);
            }


            if (!dtos.IsNullOrEmpty())
            {
                foreach (var item in dtos)
                {

                    switch (item.Event)
                    {
                        case "ABANDON":
                            item.CallDuration = 0;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data3) ? Convert.ToInt32(item.Data3) : 0;
                            item.EntryPosition = item.Data2;
                            item.QueuePosition = item.Data1;
                            break;
                        case "RINGNOANSWER":
                            item.CallDuration = 0;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data1) ? Convert.ToInt32(item.Data1)/1000 : 0;
                            break;
                        case "RINGCANCELED":
                            item.CallDuration = 0;
                            break;
                        case "COMPLETEAGENT":
                        case "COMPLETECALLER":
                            item.CallDuration = !string.IsNullOrEmpty(item.Data2) ? Convert.ToInt32(item.Data2) : 0;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data1) ? Convert.ToInt32(item.Data1) : 0;
                            item.EntryPosition = item.Data3;
                            break;
                        case "EXITWITHTIMEOUT":
                            item.CallDuration = 0;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data3) ? Convert.ToInt32(item.Data3) : 0;
                            item.EntryPosition = item.Data2;
                            item.QueuePosition = item.Data1;
                            break;
                        case "EXITWITHKEY":
                            item.CallDuration = 0;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data3) ? Convert.ToInt32(item.Data3) : 0;
                            item.EntryPosition = item.Data3;
                            item.QueuePosition = item.Data2;
                            break;
                        case "BLINDTRANSFER":
                            item.CallDuration = !string.IsNullOrEmpty(item.Data4) ? Convert.ToInt32(item.Data4) : 0; ;
                            item.CallWaiting = !string.IsNullOrEmpty(item.Data3) ? Convert.ToInt32(item.Data3) : 0;
                            item.EntryPosition = item.Data5;
                            item.QueuePosition = "0";
                            break;
                        default:
                            break;
                    }

                }
            }

            return new PaginatedList<QueueLogDto>(dtos, count, filter.PageNumber, filter.PageSize);

            

        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetQueueLogs(filter);
            }
            else
            {
                throw;

            }

        }

    }

   
}
