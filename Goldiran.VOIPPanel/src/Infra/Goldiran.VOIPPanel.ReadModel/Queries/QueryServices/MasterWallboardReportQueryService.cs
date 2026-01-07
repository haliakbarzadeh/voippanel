using AutoMapper;
using Azure;
using Azure.Core;
using Dapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Enums;
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
using Voip.Framework.EFCore;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class MasterWallboardReportQueryService : IMasterWallboardReportQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly MySqlConnection _sqlConnection;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private int _retry = 0;

    public MasterWallboardReportQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IReadModelContext readModelContext, IMapper mapper, IDistributedCache cach)
    {
        _context = context;
        _askContext = askContext;
        _sqlConnection = sqlConnection;
        _readModelContext=readModelContext;
        _mapper = mapper;
        _cach = cach;
    }

    public async Task<MasterWallboardReportDto> GetMasterWallboardData(GetMasterWallboardQuery filter)
    {
        if (filter.UserId == 1)
        {
            filter.PositionId = 11;
            filter.UserId = 37;
        }


        var up = await _readModelContext.Set<UserPosition>().AsNoTracking().Where(c => c.IsActive && c.UserId == filter.UserId && c.PositionId == filter.PositionId).FirstOrDefaultAsync();
        var queueCodes=up.Queues.Split(',').Select(c=>Convert.ToInt32(c)).ToList();

        var slaQueues = _readModelContext.Set<Queu>().AsNoTracking().Where(c => queueCodes.Contains(c.Code) && c.IsSLA).ToList();
        var fcrQueues = _readModelContext.Set<Queu>().AsNoTracking().Where(c => queueCodes.Contains(c.Code) && c.IsFCR).Select(c=>c.Code.ToString()).ToList();



        var wallboardListDto = new List<WallboardDto>();
        var dateKey = ((DateTime)filter.FromDate).Date.Add(new TimeSpan(23, 59, 59));

        string morning = filter.IsMorningShift != null ? filter.IsMorningShift.ToString() : "null";
        string key = $"{filter.UserId}_mw_{(dateKey).Date}_{morning}";

        try
        {
            var cachResult = _cach.Get<MasterWallboardReportDto>(key);
            if (cachResult != null)
            {
                return cachResult;
            }

        }
        catch (Exception)
        {

        }

        var agents = await GetUserPosition(filter);

        var agentExtensions =agents.Select(c=>c.Extension).ToList();
        
        var response = new MasterWallboardReportDto();

        DateTime fromDate =new DateTime(filter.FromDate.Value.Year, filter.FromDate.Value.Month, 1,0,0,0);
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;
        response.CSAT=await GetCSAT(fromDate, toDate, agentExtensions.Select(c=>Convert.ToInt32(c)).ToList());
        _retry = 0;
        response.SL = await GetSl(fromDate, toDate, slaQueues.Select(c => c.Code.ToString()).ToList(), slaQueues.Select(c => c.Name).ToList());

        _retry = 0;
        fromDate = ((DateTime)filter.FromDate).Date.Add(new TimeSpan(0, 0, 0));
        response.ATT = await GetATT(fromDate, toDate, agentExtensions);
        _retry = 0;
        response.FCR = await GetFCR(fromDate.AddDays(-1), toDate, fcrQueues);


        try
        {
            var expireTime = new TimeSpan(23, 59, 59) - DateTime.Now.TimeOfDay;
            _cach.Set(key.ToLower(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expireTime });
        }
        catch (Exception ex)
        {

        }
        return response;
    }

    private async Task<decimal?> GetCSAT(DateTime? fromDate, DateTime? toDate, List<int> agents)
    {
        try
        {
            var csat = await _askContext.AskCustomers.AsNoTracking().Where(c => (c.Response == "4" || c.Response == "5") &&
                                                                     (fromDate != null ? c.Date >= fromDate : true) &&
                                                                     (toDate != null ? c.Date <= toDate : true) &&
                                                                     (agents.Contains(c.Agent))).CountAsync();

            var total = await _askContext.AskCustomers.AsNoTracking().Where(c =>
                                                                         (fromDate != null ? c.Date >= fromDate : true) &&
                                                                         (toDate != null ? c.Date <= toDate : true) &&
                                                                         (agents.Contains(c.Agent))).CountAsync();
            if (total == 0)
                return 0;
            else
                return Math.Truncate((decimal)(csat * 100) / total);
        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetCSAT(fromDate, toDate, agents);
            }
            else
            {
                throw;

            }

        }

    }

    private async Task<int?> GetATT(DateTime? fromDate, DateTime? toDate, List<string> agents)
    {
        try
        {
            var result = await _context.ContactDetails.AsNoTracking().Where(c => c.Source.Length > 6 && c.Disposition == "ANSWERED" && c.Duration != null &&
                                                                                 (c.ReportType == ReportType.AutoDial ? c.Duration > 30 : true) &&
                                                                                 (fromDate != null ? c.Date >= fromDate : true) &&
                                                                                 (toDate != null ? c.Date <= toDate : true) &&
                                                                                 (agents.Contains(c.Dest))).AverageAsync(c => c.Duration);
            return result != null ? Convert.ToInt32(result) : null;
        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetATT(fromDate, toDate, agents);
            }
            else
            {
                throw;

            }

        }

    }

    private async Task<decimal?> GetFCR(DateTime fromDate, DateTime? toDate, List<string> queueCodes)
    {
        //var fcr = await _context.ContactDetails.AsNoTracking().Where(c => c.Source.Length > 6 && c.Disposition == "ANSWERED" && c.ReportType == ReportType.Normal &&
        //                                                                     (fromDate != null ? c.Date >= fromDate : true) &&
        //                                                                     (toDate != null ? c.Date <= toDate : true) &&
        //                                                                     (agents.Contains(c.Dest))).GroupBy(c=>c.Source).Where(d=>d.Count()>1).CountAsync();

        //var total = await _context.ContactDetails.AsNoTracking().Where(c => c.Source.Length > 6 && c.Disposition == "ANSWERED" && c.ReportType == ReportType.Normal &&
        //                                                             (fromDate != null ? c.Date >= fromDate.AddDays(-1) : true) &&
        //(agents.Contains(c.Dest))).GroupBy(c => c.Source).CountAsync();

        try
        {
            if (_sqlConnection.State != ConnectionState.Open)
                _sqlConnection.Open();

            var query = GetFCRQuery(fromDate, toDate, queueCodes, false);
            int fcr = 0;
            try
            {

                fcr = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 120);
            }
            catch (Exception)
            {
            }

            query = GetFCRQuery(fromDate, toDate, queueCodes, true);
            int total = 0;
            try
            {

                total = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 120);
            }
            catch (Exception)
            {
            }

                    await _sqlConnection.CloseAsync();

            if (total == 0)
                return 0;
            else
            {
                return Math.Truncate((decimal)((fcr * 100) / total));
            }
        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetFCR(fromDate, toDate, queueCodes);
            }
            else
            {
                throw;

            }

        }

    }

    private async Task<decimal?> GetSl(DateTime? fromDate, DateTime? toDate,List<string> queueCodes, List<string> queueNames)
    {
        try
        {

            List<string> eventList = new List<string>() { "COMPLETEAGENT", "COMPLETECALLER" };


            var am = await _askContext.AnsweringMachines.AsNoTracking().Where(c => (fromDate != null ? c.Date >= fromDate : true) &&
                                                                                 (toDate != null ? c.Date <= toDate : true) &&
                                                                                 queueCodes.Any(d => c.Queue == d)
                                                                                 ).CountAsync();

            var slNum = await _askContext.QueueLogs.AsNoTracking().Where(c => (fromDate != null ? c.Created >= fromDate : true) &&
                                                                         (toDate != null ? c.Created <= toDate : true) &&
                                                                         queueCodes.Any(d => c.QueueNumber == d) && eventList.Any(d => c.Event == d) &&
                                                                         (Convert.ToInt32(c.Data1) <= 20)
                                                                         ).Select(c => c.CallId).CountAsync();

            var an = await _askContext.QueueLogs.AsNoTracking().Where(c => (fromDate != null ? c.Created >= fromDate : true) &&
                                                                 (toDate != null ? c.Created <= toDate : true) &&
                                                                 queueCodes.Any(d => c.QueueNumber == d) && eventList.Any(d => c.Event == d)
                                                                 ).Select(c => c.CallId).CountAsync();


            if (an + am == 0)
                return 0;
            else
            {
                return Math.Truncate((decimal)((slNum * 100) / (an + am)));
            }
        }
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetSl(fromDate, toDate, queueCodes,queueNames);
            }
            else
            {
                throw;

            }

        }


    }
    private async Task<List<UserPosition>> GetUserPosition(GetMasterWallboardQuery filter)
    {

        var agents = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (!filter.PositionIds.IsNullOrEmpty()?filter.PositionIds.Contains(c.PositionId):true) &&
                            (!filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).ToListAsync();

        return agents;
    }

    private async Task<string?> GetImage(Guid guid)
    {

        var result= await _context.File.Include(c=>c.FileContent).AsNoTracking().Where(c => c.FileOwnerId == guid).FirstOrDefaultAsync();
        if (result != null)
            return $"data:{result.ContentType};base64,{Convert.ToBase64String(result.FileContent.Content)}";
        else
            return null;
    }

    private string GetFCRQuery(DateTime fromDate, DateTime? toDate, List<string> queueCodes, bool isTotall)
    {
        string queues=$"({string.Join(",", queueCodes)})";
        string query = $"select count(result.count) from(SELECT q.data2,count(q.data2) as count FROM asteriskcdrdb.queue_log as q where Length(q.data2>6) and q.event='ENTERQUEUE'  ";

        if (!queueCodes.IsNullOrEmpty())
        {
            query += $" and q.queuename in {queues} ";
        }

        if (fromDate != null)
        {
            
            query += $" and q.created >='{((DateTime)fromDate).ToString("yyyy-MM-dd HH:mm:ss")}' ";
        }
        if (toDate != null)
        {
            query += $" and q.created <='{((DateTime)toDate).ToString("yyyy-MM-dd HH:mm:ss")}' ";

        }

        if (!isTotall)
        {
            query += $" and exists (select  qq.callid from asteriskcdrdb.queue_log as qq where q.callid=qq.callid and qq.event  IN ('COMPLETEAGENT','COMPLETECALLER') AND TIMESTAMPDIFF(MINUTE, q.created, qq.created)<1440) group by q.data2 " +
                     " having  count(q.data2) > 1 order by q.created ";
        }
        else
        {
            query += $" and exists (select  qq.callid from asteriskcdrdb.queue_log as qq where q.callid=qq.callid and qq.event  IN ('COMPLETEAGENT','COMPLETECALLER')) group by q.data2 order by q.created ";

        }

        query +=") as result ";

        return query;
    }
}
