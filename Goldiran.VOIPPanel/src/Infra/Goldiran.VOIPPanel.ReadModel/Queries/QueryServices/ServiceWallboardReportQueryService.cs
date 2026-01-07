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
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using System.Text;
using Voip.Framework.Caching.Caching;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class ServiceWallboardReportQueryService : IServiceWallboardReportQueryService
{
    private VOIPPanelReadModelContext _context;
    private AsteriskReadModelContext _asteriskContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private readonly SqlConnection _sqlConnection;
    private readonly IConfiguration _configuration;
    private int _retry = 0;


    public ServiceWallboardReportQueryService(VOIPPanelReadModelContext context, AsteriskReadModelContext asteriskContext, IMapper mapper, IDistributedCache cach, SqlConnection sqlConnection, IConfiguration configuration)
    {
        _context = context;
        _asteriskContext = asteriskContext;
        _mapper = mapper;
        _cach = cach;
        _sqlConnection = sqlConnection;
        _configuration = configuration;
    }

    public async Task<ServiceWallboardReportDto> GetServiceWallboardsInfo(GetServiceWallboardsQuery filter)
    {
        if (_sqlConnection.State != ConnectionState.Open)
        {
            _sqlConnection.ConnectionString = _configuration.GetValue<string>("ConnectionStrings:GSReadModelConnection");
            _sqlConnection.Open();
        }
        ServiceWallboardReportDto dto = new ServiceWallboardReportDto();

        DateTime nowDate = DateTime.Now;
        DateTime fromDate;
        DateTime toDate;
        if (nowDate.Day > 1)
        {
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1, 0, 0, 0);
            toDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day - 1, 23, 59, 59);
        }
        else
        {
            DateTime date = nowDate.AddDays(-1);
            fromDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
            toDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

        }
        string key = $"wallboardservice";
        try
        {
            var cachResult = _cach.Get<ServiceWallboardReportDto>(key);
            if (cachResult != null)
            {
                return cachResult;
            }

        }
        catch (Exception)
        {

        }


        var serviceUsers = await GetPersonalCodeList(22);
        var tamdisUsers = await GetPersonalCodeList(23);

        dto.AcceptenceRate = new AcceptenceDto()
        {
            ServiceAcceptance = await GetServiceNumber("22", fromDate, toDate, serviceUsers),
            RegarantAcceptance = await GetTamdidNumber("25", fromDate, toDate, tamdisUsers)
        };

        dto.ServiceTarget = await GetTargetDto("(20,4,24)", fromDate, toDate, serviceUsers);
        dto.RegarantTarget = await GetTargetDto("(7)", fromDate, toDate, tamdisUsers);

        dto.ServiceUsersList = await GetWallboardUserList("(20,4,24)", fromDate, toDate, serviceUsers);
        dto.RegarantUsersList = await GetWallboardUserList("(7)", fromDate, toDate, tamdisUsers);

        dto.ServiceBestUser = await GetBestServiceWallboardUser("(20,4,24)", new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0), nowDate, serviceUsers);
        dto.RegarantBestUser = await GetBestTamdidWallboardUser("(7)", new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0), nowDate, tamdisUsers);

        dto.ConversionRateList = await GetConversionRateList("(4,7,20,24)", new List<string>() { "22", "25" }, new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0), nowDate);

        try
        {
            var expireTime = new TimeSpan(0, 15, 0);
            _cach.Set(key.ToLower(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expireTime });
        }
        catch (Exception ex)
        {

        }
        await _sqlConnection.CloseAsync();

        return dto;
    }

    private async Task<TargetDto> GetTargetDto(string serviceType, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());

        TargetDto dto = new TargetDto();
        if (serviceType == "(20,4,24)")
        {
            dto.Target = _configuration.GetValue<int>("GS:Service");
            dto.count = await GetTargetServiceCount(fromDate, toDate, pesonalCodes);
        }
        else
        {
            dto.Target = _configuration.GetValue<int>("GS:Regarant");
            dto.count = await GetTargetTamdidCount(fromDate, toDate, pesonalCodes);

        }




        return dto;
    }

    private async Task<int> GetServiceNumber(string queueName, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());
        int closeServiceCount = await GetClosedServiceCount(fromDate, toDate, $"('{pesonalCodes}')");

        var agents = users.Select(c => c.Extension).ToList();
        int successCall = await getSuccessCall(queueName, fromDate, toDate, agents);

        if (successCall == 0)
        {
            return 0;
        }
        else
        {
            return (closeServiceCount * 100) / successCall;

        }
    }

    private async Task<int> GetTamdidNumber(string queueName, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());
        int closeServiceCount = await GetClosedTamdidCount(fromDate, toDate, $"('{pesonalCodes}')");

        var agents = users.Select(c => c.Extension).ToList();
        int successCall = await getSuccessCall(queueName, fromDate, toDate, agents);

        if (successCall == 0)
        {
            return 0;
        }
        else
        {
            return (closeServiceCount * 100) / successCall;

        }
    }

    private async Task<int> GetClosedServiceCount(DateTime fromDate, DateTime toDate, string personalCodes)
    {
        string query = $"Select sum(count) from ( select 1 as temp, count(CreateByUser) as count from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                        $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $" and  emp.EmployeeCode in {personalCodes}  and ((ServiceTypeID in (20,4,24))) " +
                        $" union "+
                        $" select 2 as temp, sum(WarrantyExtendedTimeType) as count from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users " +
                        $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $" and ((ServiceTypeID in (7) and SMSPlanType in (4,12,13) and  emp.EmployeeCode in {personalCodes}))) as t ";

        int count = 0;
        try
        {

            count = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 80);
        }
        catch (Exception)
        {
        }

        return count;

    }

    private async Task<int> GetClosedTamdidCount(DateTime fromDate, DateTime toDate, string personalCodes)
    {
        string query = $"Select sum(count) from ( select 1 as temp, count(CreateByUser) as count from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                        $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $" and ((ServiceTypeID in (20,4,24))) and  emp.EmployeeCode in {personalCodes} " +
                        $" union " +
                        $" select 2 as temp, sum(WarrantyExtendedTimeType) as count from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users " +
                        $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $" and  emp.EmployeeCode in {personalCodes}  and ((ServiceTypeID in (7) and SMSPlanType in (4,12,13)))) as t ";

        int count = 0;
        try
        {

            count = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 80);
        }
        catch (Exception)
        {
        }

        return count;

    }

    private async Task<int> GetTargetServiceCount(DateTime fromDate, DateTime toDate, string personalCodes)
    {
        string query = $"select count(CreateByUser) from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                        $" on Service.CreateByUser=users.userId inner join [GoldServiceMain].[dbo].[aspnet_Users] as userinfo on userinfo.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $"and ServiceTypeID in (20,4,24) and LEFT(userinfo.username, 1) NOT LIKE '[0-9]'";

        int count = 0;
        try
        {

            count = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 80);
        }
        catch (Exception)
        {
        }

        return count;

    }

    private async Task<int> GetTargetTamdidCount(DateTime fromDate, DateTime toDate, string personalCodes)
    {
        string query = $"select sum(WarrantyExtendedTimeType) from (select case WarrantyExtendedTimeType when null  then 1 when 0  then 1 else WarrantyExtendedTimeType end as WarrantyExtendedTimeType from DanaPardazWebGS_Service as service  inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                        $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7  and CloseDt>='{fromDate}' and CloseDt<='{toDate}' " +
                        $"and ((ServiceTypeID in (7) and SMSPlanType in (4,12,13)))) as t";

        int count = 0;
        try
        {

            count = await _sqlConnection.QueryFirstAsync<int>(query, null, null, 80);
        }
        catch (Exception)
        {
        }

        return count;

    }

    private async Task<int> getSuccessCall(string queueName, DateTime fromDate, DateTime toDate, List<string> agents)
    {
        var eventList = new List<string>() { "COMPLETEAGENT", "COMPLETECALLER" };

        //var count = await _asteriskContext.QueueLogs.AsNoTracking().Where(c => c.QueueNumber == queueName && c.Created >= fromDate && c.Created <= toDate && eventList.Contains(c.Event) && !string.IsNullOrEmpty(c.Data2) && Convert.ToInt32(c.Data2) > 20).CountAsync();

        var count = await _context.ContactDetails.AsNoTracking().Where(c => (c.ReportType == ReportType.Normal && c.Disposition == "ANSWERED" && c.Duration > 20 && c.Source.Length < 6 && c.Dest.Length > 9 && agents.Contains(c.Source) && c.Date >= fromDate && c.Date <= toDate) ||
        (c.ReportType == ReportType.AutoDial && c.Disposition == "ANSWERED" &&  c.QueueName == queueName && c.Date >= fromDate && c.Date <= toDate && (agents.Contains(c.Source) || agents.Contains(c.Dest)) && c.Duration > 20)).CountAsync();

        return count;


    }



    private async Task<WallboardUserDto> GetBestServiceWallboardUser(string serviceType, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());

        var dto = await GetBestGsServiceWallboardUser(serviceType, fromDate, toDate,pesonalCodes);

        AppUser? user = null;

        if (dto != null)
        {
            user = await GetUser(dto.PersonalID);
        }


        if (user != null)
        {
            dto.UserName = user.UserName;
            dto.Image = await GetImage(user.Guid);
        }

        return dto;
    }

    private async Task<WallboardUserDto> GetBestTamdidWallboardUser(string serviceType, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());

        var dto = await GetBestGsTamdidWallboardUser(serviceType, fromDate, toDate, pesonalCodes);

        AppUser? user = null;

        if (dto != null)
        {
            user = await GetUser(dto.PersonalID);
        }


        if (user != null)
        {
            dto.UserName = user.UserName;
            dto.Image = await GetImage(user.Guid);
        }

        return dto;
    }

    private async Task<List<WallboardUserDto>> GetWallboardUserList(string serviceType, DateTime fromDate, DateTime toDate, List<UserPosition> users)
    {
        var pesonalCodes = string.Join("','", users.Select(c => c.User.PersonalCode).ToList());

        List<WallboardUserDto> dto = new List<WallboardUserDto>();

        if (serviceType == "(20,4,24)")
        {
            dto = await GetGsServiceWallboardUserList(serviceType, fromDate, toDate, pesonalCodes);
        }
        else
        {
            dto = await GetGsTamdidWallboardUserList(serviceType, fromDate, toDate, pesonalCodes);
        }

        foreach (var item in dto)
        {
            var user = await GetUser(item.PersonalID);

            if (user != null)
            {
                item.UserName = user.UserName;
                item.Image = await GetImage(user.Guid);
            }
        }

        return dto;
    }

    private async Task<WallboardUserDto> GetBestGsServiceWallboardUser(string serviceType, DateTime fromDate, DateTime toDate, string personalCodes)
    {
        var parameters = new
        {

        };
        //string query = $"select  emp.EmployeeCode as PersonalID ,Service.Value from (select CreateByUser as UserId,count(CreateByUser) as Value from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service]" +
        //    $" where (isReserve is null or isReserve=0) and (servicestatusid<>1 and servicestatusid<>2) and ServiceTypeID in {serviceType} and CreateDt>='{fromDate}' and CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8' ) group by CreateByUser  order by  count(CreateByUser) desc offset 0 rows fetch next 1 rows only) as Service" +
        //    $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID  ";
        string query = $"select emp.EmployeeCode as PersonalID, service.TotalValue as Value from (select temp.userid,sum( temp.Value+temp.Value2+temp.Value3) AS TotalValue" +
                       $" from (select CreateByUser as UserId,count(WarrantyExtendedTimeType)*2 as Value,0 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
                       $" where (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID=20 and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
                       $" group by CreateByUser union select CreateByUser as UserId,0 as Value,count(WarrantyExtendedTimeType) as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
                       $" where (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID in (4,24) and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
                       $" group by CreateByUser union select CreateByUser as UserId,0 as Value,0 as Value2,sum(WarrantyExtendedTimeType)*1.5 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                       $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}'" +
                       $"  and  emp.EmployeeCode in ('{personalCodes}') and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' ) group by CreateByUser) as temp group by temp.userid " +
                       $" order by  TotalValue desc offset 0 rows fetch next 1 rows only) as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId " +
                       $" left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID ";

        var result = await _sqlConnection.QueryFirstOrDefaultAsync<WallboardUserDto>(query, parameters, null, 80);


        return result;
    }

    private async Task<WallboardUserDto> GetBestGsTamdidWallboardUser(string serviceType, DateTime fromDate, DateTime toDate, string personalCodes)
    {
        var parameters = new
        {

        };
        //string query = $"select  emp.EmployeeCode as PersonalID ,Service.Value from (select CreateByUser as UserId,count(CreateByUser) as Value from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service]" +
        //    $" where (isReserve is null or isReserve=0) and (servicestatusid<>1 and servicestatusid<>2) and ServiceTypeID in {serviceType} and CreateDt>='{fromDate}' and CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8' ) group by CreateByUser  order by  count(CreateByUser) desc offset 0 rows fetch next 1 rows only) as Service" +
        //    $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID  ";
        string query = $"select emp.EmployeeCode as PersonalID, service.TotalValue as Value from (select temp.userid,sum( temp.Value+temp.Value2+temp.Value3) AS TotalValue" +
            $" from (select CreateByUser as UserId,sum(WarrantyExtendedTimeType) as Value,0 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" where (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' ) " +
            $" and PlanType=1 group by CreateByUser union select CreateByUser as UserId,0 as Value,sum(WarrantyExtendedTimeType)*1.5 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" where (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
            $" and PlanType=5 group by CreateByUser union select CreateByUser as UserId,0 as Value,0 as Value2,count(WarrantyExtendedTimeType)*0.8 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID" +
            $" where emp.include=1 and emp.isactive=1 and emp.EmployeeCode in ('{personalCodes}') and (isReserve is null or isReserve=0) and servicestatusid=7 and ServiceTypeID in (4) and service.CreateDt>='{fromDate}' and service.CreateDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
            $" group by CreateByUser) as temp group by temp.userid" +
            $" order by  TotalValue desc offset 0 rows fetch next 1 rows only) as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
            $" on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID ";

        var result = await _sqlConnection.QueryFirstOrDefaultAsync<WallboardUserDto>(query, parameters, null, 80);


        return result;
    }
    private async Task<List<WallboardUserDto>> GetGsServiceWallboardUserList(string serviceType, DateTime fromDate, DateTime toDate, string personalCodes)
    {
        var parameters = new
        {

        };
        //string query = $"select  emp.EmployeeCode as PersonalID,Service.Value from (select CreateByUser as UserId,count(CreateByUser) as Value from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service]" +
        //    $" where servicestatusid=7 and ServiceTypeID in {serviceType} and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8' ) group by CreateByUser order by  count(CreateByUser) desc offset 0 rows fetch next 7 rows only) as Service" +
        //    $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where users.PersonalID<>42133 and users.PersonalID<>10887";

        string query = $"select emp.EmployeeCode as PersonalID, service.TotalValue as Value from (select temp.userid,sum( temp.Value+temp.Value2+temp.Value3) AS TotalValue" +
                       $" from (select CreateByUser as UserId,count(WarrantyExtendedTimeType)*2 as Value,0 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
                       $" where servicestatusid=7 and ServiceTypeID=20 and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
                       $" group by CreateByUser union select CreateByUser as UserId,0 as Value,count(WarrantyExtendedTimeType) as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
                       $" where servicestatusid=7 and ServiceTypeID in (4,24) and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
                       $" group by CreateByUser union select CreateByUser as UserId,0 as Value,0 as Value2,sum(WarrantyExtendedTimeType)*1.5 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
                       $" on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where emp.include=1 and emp.isactive=1 and servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and CloseDt>='{fromDate}' and CloseDt<='{toDate}'" +
                       $"  and  emp.EmployeeCode in ('{personalCodes}') and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' ) group by CreateByUser) as temp group by temp.userid " +
                       $" order by  TotalValue desc offset 0 rows fetch next 7 rows only) as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId " +
                       $" left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID ";

        var result = await _sqlConnection.QueryAsync<WallboardUserDto>(query, parameters, null, 80);


        return result.ToList();
    }

    private async Task<List<WallboardUserDto>> GetGsTamdidWallboardUserList(string serviceType, DateTime fromDate, DateTime toDate, string personalCodes)
    {
        var parameters = new
        {

        };
        //string query = $"select  emp.EmployeeCode as PersonalID,Service.Value from (select CreateByUser as UserId,count(CreateByUser) as Value from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service]" +
        //    $" where servicestatusid=7 and ServiceTypeID in {serviceType} and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8' ) group by CreateByUser order by  count(CreateByUser) desc offset 0 rows fetch next 7 rows only) as Service" +
        //    $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID where users.PersonalID<>42133 and users.PersonalID<>10887";

        string query = $"select emp.EmployeeCode as PersonalID, service.TotalValue as Value from (select temp.userid,sum( temp.Value+temp.Value2+temp.Value3) AS TotalValue" +
            $" from (select CreateByUser as UserId,sum(WarrantyExtendedTimeType) as Value,0 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" where servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' ) " +
            $" and PlanType=1 group by CreateByUser union select CreateByUser as UserId,0 as Value,sum(WarrantyExtendedTimeType)*1.5 as Value2,0 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" where servicestatusid=7 and ServiceTypeID=7 and SMSPlanType in (4,12,13) and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
            $" and PlanType=5 group by CreateByUser union select CreateByUser as UserId,0 as Value,0 as Value2,count(WarrantyExtendedTimeType)*0.8 as value3 from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] as service" +
            $" inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users on Service.CreateByUser=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID" +
            $" where emp.include=1 and emp.isactive=1 and emp.EmployeeCode in ('{personalCodes}') and servicestatusid=7 and ServiceTypeID in (4,20,24) and CloseDt>='{fromDate}' and CloseDt<='{toDate}' and CreateByUser not in ('FAD5923A-CEA3-4A81-BBD2-00B7AE059F41','534BC383-D137-43D1-91A8-6E7EF5C173E3', '8945A3FD-7690-4939-AB15-A366C79618E8','488081EA-2E7F-4EAF-B525-1694C268FF15' )" +
            $" group by CreateByUser) as temp group by temp.userid" +
            $" order by  TotalValue desc offset 0 rows fetch next 7 rows only) as service inner join [GoldServiceMain].[dbo].[DanaPardazWebGS_User] as users" +
            $" on Service.userId=users.userId left join [GoldServiceMain].[dbo].[DanaPardazWebGS_Employee] as emp on users.PersonalID=emp.PersonalID ";

        var result = await _sqlConnection.QueryAsync<WallboardUserDto>(query, parameters, null, 80);


        return result.ToList();
    }


    private async Task<List<ConversionRateDto>> GetConversionRateList(string serviceType, List<string> queueName, DateTime fromDate, DateTime toDate)
    {
        var closeServiceCount = await GetGsConversionRateList(serviceType, fromDate, toDate);
        var serviceSuccessCall = await getConversationSuccessCall(new List<string>() { "22"}, fromDate, toDate);
        var tamdidSuccessCall = await getConversationSuccessCall(new List<string>() { "25" }, fromDate, toDate);


        foreach (var conversionRate in closeServiceCount)
        {
            if (serviceSuccessCall.ContainsKey(conversionRate.TimeDuration))
            {
                conversionRate.ServiceCallCount = serviceSuccessCall[conversionRate.TimeDuration];

            }
            else
            {
                conversionRate.ServiceCallCount = 0;
            }
            conversionRate.CallCount = conversionRate.ServiceCallCount;
            if (serviceSuccessCall.ContainsKey(conversionRate.TimeDuration))
            {
                try
                {
                    conversionRate.TamdidCallCount = tamdidSuccessCall[conversionRate.TimeDuration];
                }
                catch (Exception ex)
                {

                    conversionRate.TamdidCallCount = 0;
                }


            }
            else
            {
                conversionRate.TamdidCallCount = 0;
            }

        }

        return closeServiceCount;
    }
    private async Task<List<ConversionRateDto>> GetGsConversionRateList(string serviceType, DateTime fromDate, DateTime toDate)
    {
        var parameters = new
        {

        };
        string query = $"select t.TimeDuration, sum(ServiceCount) as ServiceCount, sum(TamdidCount) as TamdidCount from ( select  DATEPART(HOUR, CreateDt) as TimeDuration,count(   DATEPART(HOUR, CreateDt)) as ServiceCount, 0 as TamdidCount  from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] " +
                       $"where (servicestatusid<>1 and servicestatusid<>2) and ServiceTypeID in (20,24,4) and CreateDt>= '{fromDate}' and CreateDt<= '{toDate}'  and DATEPART(HOUR, CreateDt) >= 9 and DATEPART(HOUR, CreateDt)<= 17 " +
                       $"group by DATEPART(HOUR, CreateDt) " +
                       $"union " +
                       $"select  DATEPART(HOUR, CreateDt) as TimeDuration,0 as ServiceCount, count(DATEPART(HOUR, CreateDt)) as TamdidCount  from [GoldServiceMain].[dbo].[DanaPardazWebGS_Service] " +
                       $"where (servicestatusid<>1 and servicestatusid<>2) and ServiceTypeID =7 and SMSPlanType in (4,12,13) and CreateDt>= '{fromDate}' and CreateDt<= '{toDate}'  and DATEPART(HOUR, CreateDt) >= 9 and DATEPART(HOUR, CreateDt)<= 17 " +
                       $"group by DATEPART(HOUR, CreateDt)) as t group by TimeDuration order by DATEPART(HOUR, TimeDuration)";

        var result = await _sqlConnection.QueryAsync<ConversionRateDto>(query, parameters, null, 80);


        return result.ToList();
    }

    private async Task<Dictionary<int, int>> getConversationSuccessCall(List<string> queueName, DateTime fromDate, DateTime toDate)
    {
        var result = new Dictionary<int, int>();
        var eventList = new List<string>() { "COMPLETEAGENT", "COMPLETECALLER" };

        var list = await _asteriskContext.QueueLogs.AsNoTracking().Where(c => queueName.Contains(c.QueueNumber) && c.Created >= fromDate && c.Created <= toDate && eventList.Contains(c.Event) && !string.IsNullOrEmpty(c.Data2) && Convert.ToInt32(c.Data2) > 20 && c.Created.Hour >= 9 && c.Created.Hour <= 17).GroupBy(c => c.Created.Hour).ToListAsync();

        foreach (var item in list)
        {
            result.Add(item.Key, item.Count());
        }

        return result;

    }

    private async Task<AppUser?> GetUser(string personalCode)
    {

        return await _context.User.AsNoTracking().Where(c => c.PersonalCode == personalCode).FirstOrDefaultAsync();

    }

    private async Task<string?> GetImage(Guid guid)
    {

        var result = await _context.File.Include(c => c.FileContent).AsNoTracking().Where(c => c.FileOwnerId == guid).FirstOrDefaultAsync();
        if (result != null)
            return $"data:{result.ContentType};base64,{Convert.ToBase64String(result.FileContent.Content)}";
        else
            return null;
    }

    private async Task<List<UserPosition>> GetPersonalCodeList(long posId)
    {
        List<string> personalCode=new List<string>();

        if (posId == 22)
        {
            personalCode = _configuration.GetValue<string>("GS:ServiceUsers").Split(",").ToList();
        }
        else
        {
            personalCode = _configuration.GetValue<string>("GS:TamdidUsers").Split(",").ToList();

        }

        return await _context.UserPosition.Include(c => c.User).AsNoTracking().Where(c => c.PositionId == posId && c.IsActive && personalCode.Contains(c.User.PersonalCode)).ToListAsync();

    }

}


