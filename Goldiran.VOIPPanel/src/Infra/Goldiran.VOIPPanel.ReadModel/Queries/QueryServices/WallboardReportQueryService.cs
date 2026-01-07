using AutoMapper;
using Azure;
using Azure.Core;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Voip.Framework.Caching.Caching;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class WallboardReportQueryService : IWallboardReportQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private int _retry = 0;


    public WallboardReportQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, IMapper mapper, IDistributedCache cach)
    {
        _context = context;
        _askContext = askContext;
        _mapper = mapper;
        _cach = cach;
    }

    public async Task<WallboardReportDto> GetAgentContactsCount(GetWallboardsQuery filter)
    {
        if (filter.UserId == 1)
        {
            filter.PositionId = 11;
            filter.UserId = 37;
        }
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;
        var wallboardListDto = new List<WallboardDto>();

        string morning = filter.IsMorningShift != null ? filter.IsMorningShift.ToString() : "null";
        string key = $"{filter.UserId}_{filter.wallboardReportType}_{((DateTime)fromDate).Date}_{morning}";

        try
        {
            var cachResult = _cach.Get<WallboardReportDto>(key);
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
        
        var result = await _context.ContactDetails.AsNoTracking().Where(c => (agentExtensions.Contains(c.Dest) && c.Source.Length>5 && c.Disposition=="ANSWERED") &&
                                                                             (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                                                             (filter.ToDate != null ? c.Date <= toDate : true) ).GroupBy(x => x.Dest).ToListAsync();

        
        foreach (var item in result) 
        {
            var pos= agents.Where(c => c.Extension == item.Key).FirstOrDefault();

            var walboardDto=new WallboardDto();
            walboardDto.UserName = pos.User.UserName;
            walboardDto.FullName = pos.User.PersianFullName;
            walboardDto.Value = item.Count();
            walboardDto.ValueStr= walboardDto.Value.ToString();
            walboardDto.Image = await GetImage(pos.User.Guid);

            wallboardListDto.Add(walboardDto);

        }

        wallboardListDto = wallboardListDto.OrderByDescending(c => c.Value).ToList();

        var response = new WallboardReportDto() { ItemList = wallboardListDto };

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

    public async Task<WallboardReportDto> GetCallAverageTime(GetWallboardsQuery filter)
    {
        if (filter.UserId == 1)
        {
            filter.PositionId = 11;
            filter.UserId = 37;
        }
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;
        var wallboardListDto = new List<WallboardDto>();

        string morning = filter.IsMorningShift != null ? filter.IsMorningShift.ToString() : "null";
        string key = $"{filter.UserId}_{filter.wallboardReportType}_{((DateTime)fromDate).Date}_{morning}";

        try
        {
            var cachResult = _cach.Get<WallboardReportDto>(key);
            if (cachResult != null)
            {
                return cachResult;
            }

        }
        catch (Exception)
        {

        }

        var agents = await GetUserPosition(filter);

        var agentExtensions = agents.Select(c => c.Extension).ToList();

        var result = await _context.ContactDetails.AsNoTracking().Where(c => (agentExtensions.Contains(c.Dest) && c.Source.Length > 5 && c.Disposition == "ANSWERED") &&
                                                                             (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                                                             (filter.ToDate != null ? c.Date <= toDate : true)).GroupBy(x => x.Dest).ToListAsync();
       
        foreach (var item in result)
        {
            var pos = agents.Where(c => c.Extension == item.Key).FirstOrDefault();

            var walboardDto = new WallboardDto();
            walboardDto.UserName = pos.User.UserName;
            walboardDto.FullName = pos.User.PersianFullName;
            walboardDto.Value = Convert.ToInt32(item.Select(c => c.Duration).Average());
            var durationTimeSpan = TimeSpan.FromSeconds((int)walboardDto.Value);
            walboardDto.ValueStr = $"{durationTimeSpan.Minutes.ToString("00")}:{durationTimeSpan.Seconds.ToString("00")}";
            walboardDto.Image = await GetImage(pos.User.Guid);

            wallboardListDto.Add(walboardDto);

        }

        wallboardListDto = wallboardListDto.OrderByDescending(c => c.Value).ToList();

        var response = new WallboardReportDto() { ItemList = wallboardListDto };

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

    public async Task<WallboardReportDto> GetCallSumTime(GetWallboardsQuery filter)
    {
        if (filter.UserId == 1)
        {
            filter.PositionId = 11;
            filter.UserId = 37;
        }
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;
        var wallboardListDto = new List<WallboardDto>();

        string morning = filter.IsMorningShift != null ? filter.IsMorningShift.ToString() : "null";
        string key = $"{filter.UserId}_{filter.wallboardReportType}_{((DateTime)fromDate).Date}_{morning}";

        try
        {
            var cachResult = _cach.Get<WallboardReportDto>(key);
            if (cachResult != null)
            {
                return cachResult;
            }

        }
        catch (Exception)
        {

        }

        var agents = await GetUserPosition(filter);

        var agentExtensions = agents.Select(c => c.Extension).ToList();

        var result = await _context.ContactDetails.AsNoTracking().Where(c => (agentExtensions.Contains(c.Dest) && c.Source.Length > 5 && c.Disposition == "ANSWERED" && c.Duration!=null) &&
                                                                             (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                                                             (filter.ToDate != null ? c.Date <= toDate : true)).GroupBy(x => x.Dest).ToListAsync();

        foreach (var item in result)
        {
            var pos = agents.Where(c => c.Extension == item.Key).FirstOrDefault();

            var walboardDto = new WallboardDto();
            walboardDto.UserName = pos.User.UserName;
            walboardDto.FullName = pos.User.PersianFullName;
            walboardDto.Value = item.Select(c => c.Duration).Sum(c => c.Value);
            walboardDto.Image = await GetImage(pos.User.Guid);


            var durationTimeSpan = TimeSpan.FromSeconds((int)walboardDto.Value);
            walboardDto.ValueStr = $"{durationTimeSpan.Hours.ToString("00")}:{durationTimeSpan.Minutes.ToString("00")}:{durationTimeSpan.Seconds.ToString("00")}";

            wallboardListDto.Add(walboardDto);

        }

        wallboardListDto = wallboardListDto.OrderByDescending(c => c.Value).ToList();

        var response = new WallboardReportDto() { ItemList = wallboardListDto };

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

    public async Task<WallboardReportDto> GetCSAT(GetWallboardsQuery filter)
    {
        try
        {
            if (filter.UserId == 1)
            {
                filter.PositionId = 11;
                filter.UserId = 37;
            }
            DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
            DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;
            var wallboardListDto = new List<WallboardDto>();

            string morning = filter.IsMorningShift != null ? filter.IsMorningShift.ToString() : "null";
            string key = $"{filter.UserId}_{filter.wallboardReportType}_{((DateTime)fromDate).Date}_{morning}";

            try
            {
                var cachResult = _cach.Get<WallboardReportDto>(key);
                if (cachResult != null)
                {
                    return cachResult;
                }

            }
            catch (Exception)
            {

            }

            var agents = await GetUserPosition(filter);

            var agentExtensions = agents.Select(c => Convert.ToInt32(c.Extension)).ToList();

            var result = await _askContext.AskCustomers.AsNoTracking().Where(c => agentExtensions.Contains(c.Agent) && c.Response!=string.Empty &&
                                                                         (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                                                         (filter.ToDate != null ? c.Date <= toDate : true)).GroupBy(x => x.Agent).ToListAsync();

            foreach (var item in result)
            {
                var pos = agents.Where(c => c.Extension == item.Key.ToString()).FirstOrDefault();

                var walboardDto = new WallboardDto();
                walboardDto.UserName = pos.User.UserName;
                walboardDto.FullName = pos.User.PersianFullName;
                walboardDto.Value = Convert.ToInt32((item.Select(c => Convert.ToInt32(c.Response)).Average() * 20));

                walboardDto.ValueStr = $"{walboardDto.Value}%";
                walboardDto.Image = await GetImage(pos.User.Guid);


                wallboardListDto.Add(walboardDto);

            }

            wallboardListDto = wallboardListDto.OrderByDescending(c => c.Value).ToList();

            var response = new WallboardReportDto() { ItemList = wallboardListDto };

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
        catch (Exception ex)
        {
            if (_retry < 1)
            {
                _retry++;
                return await GetCSAT(filter);
            }
            else
            {
                throw;

            }

        }
       
    }

    public async Task<WallboardReportDto> GetOperationStatus(GetWallboardsQuery filter)
    {
        if (filter.UserId == 1)
        {
            filter.PositionId = 11;
            filter.UserId = 37;
        }
        var time=DateTime.Now.TimeOfDay; 

        OperationType operationType;
        if (filter.wallboardReportType == WallboardReportType.AnsweringOperation)
        {
            operationType = OperationType.Answering;
        }
        else 
        {
            operationType = OperationType.Rest;
        }

        //filter.PositionId = 11;
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;

        string morning= filter.IsMorningShift != null ?filter.IsMorningShift.ToString() :"null";
        string key = $"{filter.UserId}_{filter.wallboardReportType}_{Convert.ToInt16(operationType)}_{((DateTime)fromDate).Date}_{morning}";

        try
        {
            var cachResult = _cach.Get<WallboardReportDto>(key);
            if (cachResult != null)
            {
                return cachResult;
            }

        }
        catch (Exception)
        {

        }

        var wallboardListDto = new List<WallboardDto>();

        var agents = await GetUserPosition(filter);

        var agentExtensions = agents.Select(c => c.UserId).ToList();

        var result = await _context.Operation.AsNoTracking().Where(c => agentExtensions.Contains(c.UserId) && 
                                                                         (operationType!= OperationType.Rest? c.OperationTypeId==operationType:(c.OperationTypeId== OperationType.Rest || c.OperationTypeId == OperationType.Launch || c.OperationTypeId == OperationType.ACWT)) &&
                                                                         (!filter.PositionIds.IsNullOrEmpty()?filter.PositionIds.Contains(c.PositionId):true) &&
                                                                         (filter.FromDate != null ? c.StartDate >= fromDate : true) &&
                                                                         (filter.ToDate != null ? c.StartDate <= toDate : true)).GroupBy(x => x.UserId).ToListAsync();

        foreach (var item in result)
        {
            var pos = agents.Where(c => c.UserId.ToString() == item.Key.ToString()).FirstOrDefault();

            var walboardDto = new WallboardDto();
            walboardDto.UserName = pos.User.UserName;
            walboardDto.FullName = pos.User.PersianFullName;
            walboardDto.Image = await GetImage(pos.User.Guid);
            walboardDto.Value = Convert.ToInt32((item.Where(c=>c.EndDate!=null).Select(c => Convert.ToInt32(c.StatusDuration)).Sum() * 60));
            if (filter.IsMorningShift != null && (bool)filter.IsMorningShift)
            {
                walboardDto.Value += Convert.ToInt32((item.Where(c => c.EndDate == null).Select(c =>(c.StartDate.Add(time) - c.StartDate.Add(c.StartTime)).TotalSeconds).Sum()));
            }
            else if (filter.IsMorningShift != null && !(bool)filter.IsMorningShift)
            {
                walboardDto.Value += Convert.ToInt32((item.Where(c => c.EndDate == null).Select(c => (c.StartDate.Add(time) - c.StartDate.Add(c.StartTime)).TotalSeconds).Sum()));
            }
            else
            {
                walboardDto.Value += Convert.ToInt32((item.Where(c => c.EndDate == null).Select(c =>time.TotalSeconds>61200? (c.StartDate.Add(new TimeSpan(17, 0, 0)) - c.StartDate.Add(c.StartTime)).TotalSeconds : (c.StartDate.Add(time) - c.StartDate.Add(c.StartTime)).TotalSeconds).Sum()));

            }

            var durationTimeSpan = TimeSpan.FromSeconds((int)walboardDto.Value);

            walboardDto.ValueStr = $"{durationTimeSpan.Hours.ToString("00")}:{durationTimeSpan.Minutes.ToString("00")}:{durationTimeSpan.Seconds.ToString("00")}";

            wallboardListDto.Add(walboardDto);

        }

        wallboardListDto = wallboardListDto.OrderByDescending(c => c.Value).ToList();

        var response= new WallboardReportDto() { ItemList = wallboardListDto };

        try
        {
            var expireTime = new TimeSpan(23,59,59)-DateTime.Now.TimeOfDay;
            _cach.Set(key.ToLower(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expireTime });
        }
        catch (Exception ex)
        {

        }
        return response;
    }

    private async Task<List<UserPosition>> GetUserPosition(GetWallboardsQuery filter)
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
}
