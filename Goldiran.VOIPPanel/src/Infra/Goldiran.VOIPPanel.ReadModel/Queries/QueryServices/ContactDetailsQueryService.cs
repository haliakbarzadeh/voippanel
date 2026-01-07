using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class ContactDetailsQueryService : IContactDetailQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly MySqlConnection _sqlConnection;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private int _retry = 0;

    public ContactDetailsQueryService(VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        _configuration = configuration;

    }

    public async Task<PaginatedList<ContactDetailDto>> GetContactDetails(GetContactDetailsQuery filter)
    {
        try
        {
            _retry += 1;

            if (_sqlConnection.State != ConnectionState.Open)
                _sqlConnection.Open();

            string sp = string.Empty;
            if (!filter.IsJob && filter.ContactReportType == ContactReportType.AutoDial)
                sp = _configuration.GetValue<string>("IzabelConfigs:SPAutoDial");
            else if (filter.IsJob && filter.ContactReportType == ContactReportType.AutoDial)
                sp = _configuration.GetValue<string>("IzabelConfigs:FlatSPAutoDial");
            else if (filter.ContactReportType == ContactReportType.Detail && !filter.IsJob)
                sp = _configuration.GetValue<string>("IzabelConfigs:SP");
            else
                sp = _configuration.GetValue<string>("IzabelConfigs:FlatSP");

            if (filter.IsJob)
            {
                filter.Agents = null;
            }
            else if (string.IsNullOrEmpty(filter.Agents))
            {
                var agents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                    .Where(c => (c.IsActive) &&
                                (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
                filter.Agents = string.Join(",", agents);
            }
            string? typeCall = null;

            if (filter.IsJob)
            {
                typeCall = null;
            }
            else if (filter.TypeCalls.IsNullOrEmpty())
            {
                typeCall = "1,2,3,4";
            }
            else
            {
                typeCall = string.Join(",", filter.TypeCalls);
            }

            if (filter.IsJob)
            {
                filter.Phone = null;
                filter.PageNumber = 1;
                filter.OrderBy = 1;
            }
            else
            {
                filter.OrderBy = 0;

            }

            SetDuration(filter);

            var parameters = new DynamicParameters();
            parameters.Add("@startDate", (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0)), DbType.DateTime);
            if (!filter.IsJob)
                parameters.Add("@endDate", (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)), DbType.DateTime);
            else
                parameters.Add("@endDate", (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)), DbType.DateTime);
            //parameters.Add("@typeCall", !filter.TypeCalls.IsNullOrEmpty()?string.Join(',',filter.TypeCalls):null, DbType.String);
            parameters.Add("@typeCall", typeCall, DbType.String);
            parameters.Add("@phone", filter.Phone, DbType.String);
            //parameters.Add("@number", !filter.Agents.IsNullOrEmpty() ? string.Join(',', filter.Agents) : null, DbType.String);
            parameters.Add("@number", filter.Agents, DbType.String);
            parameters.Add("@pageNumber", filter.PageNumber, DbType.Int32);
            parameters.Add("@pageSize", filter.PageSize, DbType.Int32);
            parameters.Add("@orderby", filter.OrderBy, DbType.Int32);
            parameters.Add("@count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await _sqlConnection.QueryAsync<ContactDetailDto>(sp, parameters, null, 300, CommandType.StoredProcedure);
            int count = parameters.Get<int>("@count");

            await _sqlConnection.CloseAsync();

            if (count > 0)
                return new PaginatedList<ContactDetailDto>(result.ToList(), count, filter.PageNumber, filter.PageSize);
            else
                return new PaginatedList<ContactDetailDto>(new List<ContactDetailDto>(), count, filter.PageNumber, filter.PageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            //if (ex.Message.Contains("The Command Timeout expired before the operation completed.") && _retry <= 1)
            if (_retry <= 1)
            {
                return await GetContactDetails(filter);
            }
            else
            {
                throw new ValidationException(new List<string>() { "مجددا اجرا کنید" });

            }
        }

    }

    private void SetDuration(GetContactDetailsQuery filter)
    {
        List<DateTime> result = new List<DateTime>();

        if (filter.OperationReportType != null)
        {
            switch (filter.OperationReportType)
            {
                case OperationReportType.OneWeek:
                    filter.FromDate = DateTime.Now.AddDays(-7);
                    filter.ToDate = DateTime.Now;
                    break;
                case OperationReportType.TwoWeek:
                    filter.FromDate = DateTime.Now.AddDays(-14);
                    filter.ToDate = DateTime.Now;
                    break;
                case OperationReportType.OneMounth:
                    filter.FromDate = DateTime.Now.AddDays(-30);
                    filter.ToDate = DateTime.Now;
                    break;
                case OperationReportType.OneSeason:
                    filter.FromDate = DateTime.Now.AddDays(-90);
                    filter.ToDate = DateTime.Now;
                    break;
                case OperationReportType.OneYear:
                    filter.FromDate = DateTime.Now.AddYears(-1);
                    filter.ToDate = DateTime.Now;
                    break;
                default:
                    break;
            }
        }

    }

    public async Task<PaginatedList<ContactDetailDto>> GetFlatContactDetails(GetContactDetailsQuery filter)
    {
        bool flag = false;
        var position = await _context.Position.AsNoTracking().Where(c => c.Id == filter.PositionId).FirstOrDefaultAsync();

        if(position!=null && position.IsContentAccess)
        {
            flag = true;
        }

        ReportType reportType = (ReportType)((int)filter.ContactReportType);
        DateTime fromDate = (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0));
        DateTime toDate = (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59));
        var agents = new List<string>();
        if (string.IsNullOrEmpty(filter.Agents) && filter.PositionIds.IsNullOrEmpty() && !flag)
        {
            agents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
        }
        else if (!string.IsNullOrEmpty(filter.Agents))
        {
            agents = filter.Agents.Split(',').ToList();
        }

        if (!filter.PositionIds.IsNullOrEmpty() && string.IsNullOrEmpty(filter.Agents) && !flag)
        {
            var positionAgents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.PositionIds.Contains(c.PositionId)) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
            agents.AddRange(positionAgents);
        }

        var result = await _context.ContactDetails.AsNoTracking()
                        .Where(c =>
                                    (!EF.Functions.Like(c.Dest, $"IAX2%")) &&
                                    (c.ReportType == reportType) &&
                                    (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                    (filter.ToDate != null ? c.Date <= toDate : true) &&
                                    (!string.IsNullOrEmpty(filter.Phone) ? (EF.Functions.Like(c.Source, $"%{filter.Phone}%") || EF.Functions.Like(c.Dest, $"%{filter.Phone}%")) : true) &&
                                    ((flag && agents.IsNullOrEmpty()) || agents.Contains(c.Source) || agents.Contains(c.Dest)) &&
                                    (
                                    filter.TypeCalls.IsNullOrEmpty() ||
                                    (filter.TypeCalls.Contains(1) ? (c.Source.Length > 4 && c.Dest.Length < 6 && !EF.Functions.Like(c.Source, $"%*%")) : false) ||
                                    (filter.TypeCalls.Contains(2) ? (c.Source.Length < 5 && c.Dest.Length < 6) : false) ||
                                     (filter.TypeCalls.Contains(3) ? ((c.Source.Length < 5 && c.Dest.Length > 4) || (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.Source, $"%*%"))) : false) ||
                                      (filter.TypeCalls.Contains(4) ? (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.DstChannel, $"SIP/R1_%")) : false)
                                    ))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<ContactDetailDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<ContactDetailDto>> GetFlatContactDetails1(GetContactDetailsQuery filter)
    {
        DateTime fromDate = (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0));
        DateTime toDate = (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59));
        var agents = new List<string>();
        if (string.IsNullOrEmpty(filter.Agents))
        {
            agents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
        }
        else
        {
            agents = filter.Agents.Split(',').ToList();
        }
        var result = await _context.ContactDetails.AsNoTracking()
                        .Where(c => (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                    (filter.ToDate != null ? c.Date <= toDate : true) &&
                                    (!string.IsNullOrEmpty(filter.Phone) ? (EF.Functions.Like(c.Source, $"%{filter.Phone}%") || EF.Functions.Like(c.Dest, $"%{filter.Phone}%")) : true) &&
                                    (agents.Contains(c.Source) || agents.Contains(c.Dest)) &&
                                    (
                                    filter.TypeCalls.IsNullOrEmpty() ||
                                    (filter.TypeCalls.Contains(1) ? (c.Source.Length > 4 && c.Dest.Length < 6) : false) ||
                                    (filter.TypeCalls.Contains(2) ? (c.Source.Length < 5 && c.Dest.Length < 6) : false) ||
                                     (filter.TypeCalls.Contains(3) ? (c.Source.Length < 5 && c.Dest.Length > 4) : false) ||
                                      (filter.TypeCalls.Contains(4) ? (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.DstChannel, $"SIP/R1_%")) : false)
                                    ))
                        .OrderByDescending(x => x.Id)
                        .Select(c => c.LinkedId)
                        .Distinct()
                        .ProjectTo<ContactDetailDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<AutoDialDto>> GetAutoDetails(GetAutoDetailsQuery filter)
    {
        try
        {
            _retry += 1;

            if (_sqlConnection.State != ConnectionState.Open)
                _sqlConnection.Open();

            string sp = string.Empty;

            if (!filter.IsJob && filter.ContactReportType == ContactReportType.AutoDial)
                sp = _configuration.GetValue<string>("IzabelConfigs:NewSPAutoDial");
            else if (filter.IsJob && filter.ContactReportType == ContactReportType.AutoDial)
                sp = _configuration.GetValue<string>("IzabelConfigs:NewFlatSPAutoDial");

            var agents = new List<string>();

            if (!filter.IsJob && !string.IsNullOrEmpty(filter.Agents))
            {
                agents = null;
                agents = filter.Agents.Split(',').ToList();
            }

            if (filter.IsJob)
            {
                filter.Agents = null;
            }
            else if (string.IsNullOrEmpty(filter.Agents) && filter.PositionIds.IsNullOrEmpty())
            {
                var filtetAgents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                    .Where(c => (c.IsActive) &&
                                (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) ||  (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
                agents.AddRange(filtetAgents);
                //filter.Agents = string.Join(",", agents);
            }

            if (!filter.PositionIds.IsNullOrEmpty() && !filter.IsJob && string.IsNullOrEmpty(filter.Agents))
            {
                var positionAgents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                    .Where(c => (c.IsActive) &&
                                (filter.PositionIds.Contains(c.PositionId)) &&
                                (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
                agents.AddRange(positionAgents);
            }

            if (!filter.IsJob && !agents.IsNullOrEmpty())
            {
                filter.Agents = string.Join(",", agents);
            }

            string? typeCall = null;
            if (filter.IsJob)
            {
                typeCall = null;
            }
            else if (filter.TypeCalls.IsNullOrEmpty())
            {
                typeCall = "1,2,3,4";
            }
            else
            {
                typeCall = string.Join(",", filter.TypeCalls);
            }

            if (filter.IsJob)
            {
                filter.Phone = null;
                filter.PageNumber = 1;
                filter.OrderBy = 1;
            }
            else
            {
                filter.OrderBy = 0;

            }


            var parameters = new DynamicParameters();
            parameters.Add("@startDate", (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0)), DbType.DateTime);
            if (!filter.IsJob)
                parameters.Add("@endDate", (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)), DbType.DateTime);
            else
                parameters.Add("@endDate", (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)), DbType.DateTime);
            //parameters.Add("@typeCall", !filter.TypeCalls.IsNullOrEmpty()?string.Join(',',filter.TypeCalls):null, DbType.String);
            parameters.Add("@typeCall", typeCall, DbType.String);
            parameters.Add("@phone", filter.Phone, DbType.String);
            //parameters.Add("@number", !filter.Agents.IsNullOrEmpty() ? string.Join(',', filter.Agents) : null, DbType.String);
            parameters.Add("@number", filter.Agents, DbType.String);
            parameters.Add("@pageNumber", filter.PageNumber, DbType.Int32);
            parameters.Add("@pageSize", filter.PageSize, DbType.Int32);
            parameters.Add("@orderby", filter.OrderBy, DbType.Int32);
            parameters.Add("@count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await _sqlConnection.QueryAsync<AutoDialDto>(sp, parameters, null, 300, CommandType.StoredProcedure);
            int count = parameters.Get<int>("@count");

            await _sqlConnection.CloseAsync();

            if (count > 0)
                return new PaginatedList<AutoDialDto>(result.ToList(), count, filter.PageNumber, filter.PageSize);
            else
                return new PaginatedList<AutoDialDto>(new List<AutoDialDto>(), count, filter.PageNumber, filter.PageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            //if (ex.Message.Contains("The Command Timeout expired before the operation completed.") && _retry <= 1)
            if (_retry <= 1)
            {
                return await GetAutoDetails(filter);
            }
            else
            {
                throw new ValidationException(new List<string>() { "مجددا اجرا کنید" });

            }
        }
    }

    public async Task<PaginatedList<AutoDialDto>> GetFlatAutoDetails(GetAutoDetailsQuery filter)
    {
        ReportType reportType = ReportType.AutoDial;
        DateTime fromDate = (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0));
        DateTime toDate = (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59));
        var agents = new List<string>();
        var queueCodes = new List<string>();

        if (string.IsNullOrEmpty(filter.Agents) && filter.PositionIds.IsNullOrEmpty())
        {
            agents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
        }
        else if (!string.IsNullOrEmpty(filter.Agents))
        {
            agents = filter.Agents.Split(',').ToList();
        }

        if (!filter.PositionIds.IsNullOrEmpty() && string.IsNullOrEmpty(filter.Agents))
        {
            var positionAgents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.PositionIds.Contains(c.PositionId)) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
            agents.AddRange(positionAgents);
        }
        if (!filter.Queues.IsNullOrEmpty())
        {
            queueCodes = filter.Queues.Select(c=>c.ToString()).ToList();
        }

        var result = await _context.ContactDetails.AsNoTracking()
                        .Where(c =>
                                    (c.ReportType == reportType) &&
                                    (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                    (filter.ToDate != null ? c.Date <= toDate : true) &&
                                    (!string.IsNullOrEmpty(filter.Phone) ? (EF.Functions.Like(c.Source, $"%{filter.Phone}%") || EF.Functions.Like(c.Dest, $"%{filter.Phone}%")) : true) &&
                                    (agents.Contains(c.Source) || agents.Contains(c.Dest)) &&
                                    (!queueCodes.IsNullOrEmpty() ? queueCodes.Contains(c.QueueName) : true) &&
                                    (
                                    filter.TypeCalls.IsNullOrEmpty() ||
                                    (filter.TypeCalls.Contains(1) ? (c.Source.Length > 4 && c.Dest.Length < 6 && !EF.Functions.Like(c.Source, $"%*%")) : false) ||
                                    (filter.TypeCalls.Contains(2) ? (c.Source.Length < 5 && c.Dest.Length < 6) : false) ||
                                     (filter.TypeCalls.Contains(3) ? ((c.Source.Length < 5 && c.Dest.Length > 4) || (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.Source, $"%*%"))) : false) ||
                                      (filter.TypeCalls.Contains(4) ? (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.DstChannel, $"SIP/R1_%")) : false)
                                    ))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<AutoDialDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<ContactDetailDto>> GetContactDetailsByLinkedId(GetContactDetailsByLinkedIdQuery filter)
    {
        if (string.IsNullOrEmpty(filter.LinkedId))
        {
            throw new ValidationException(new List<string>
            {
                "شناسه ارسال نشده است"
            });
        }

        var result = await _context.ContactDetails.Where(x => x.LinkedId == filter.LinkedId)
            .QueryEntityResult(filter)
            .ProjectTo<ContactDetailDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<ContactDetailDto>> GetGroupedContactDetail(GetGroupedContactDetailQuery filter)
    {
        ReportType reportType = (ReportType)((int)filter.ContactReportType);
        DateTime fromDate = (filter.FromDate != null && filter.FromTime != null) ? ((DateTime)filter.FromDate).Add((TimeSpan)filter.FromTime) : ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0));
        DateTime toDate = (filter.ToDate != null && filter.ToTime != null) ? ((DateTime)filter.ToDate).Add((TimeSpan)filter.ToTime) : ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59));
        var agents = new List<string>();
        if (string.IsNullOrEmpty(filter.Agents) && filter.PositionIds.IsNullOrEmpty())
        {
            agents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
        }
        else if (!string.IsNullOrEmpty(filter.Agents))
        {
            agents = filter.Agents.Split(',').ToList();
        }

        if (!filter.PositionIds.IsNullOrEmpty())
        {
            var positionAgents = await _context.UserPosition.Include(c => c.Position).AsNoTracking()
                .Where(c => (c.IsActive) &&
                            (filter.PositionIds.Contains(c.PositionId)) &&
                            (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true)).Select(c => c.Extension).ToListAsync();
            agents.AddRange(positionAgents);
        }

        var result = await _context.ContactDetails
                        .FromSqlRaw(@"
SELECT cd.*
FROM [dbo].[ContactDetail] cd
INNER JOIN (
    SELECT LinkedId, MAX(Date) AS MaxDate
    FROM [dbo].[ContactDetail]
    GROUP BY LinkedId
) latest ON cd.LinkedId = latest.LinkedId AND cd.Date = latest.MaxDate
")
                        .Where(c =>
                                    (c.ReportType == reportType) &&
                                    (filter.FromDate != null ? c.Date >= fromDate : true) &&
                                    (filter.ToDate != null ? c.Date <= toDate : true) &&
                                    (!string.IsNullOrEmpty(filter.Phone) ? (EF.Functions.Like(c.Source, $"%{filter.Phone}%") || EF.Functions.Like(c.Dest, $"%{filter.Phone}%")) : true) &&
                                    (agents.Contains(c.Source) || agents.Contains(c.Dest)) &&
                                    (
                                    filter.TypeCalls.IsNullOrEmpty() ||
                                    (filter.TypeCalls.Contains(1) ? (c.Source.Length > 4 && c.Dest.Length < 6) : false) ||
                                    (filter.TypeCalls.Contains(2) ? (c.Source.Length < 5 && c.Dest.Length < 6) : false) ||
                                     (filter.TypeCalls.Contains(3) ? (c.Source.Length < 5 && c.Dest.Length > 4) : false) ||
                                      (filter.TypeCalls.Contains(4) ? (c.Source.Length > 4 && c.Dest.Length < 6 && EF.Functions.Like(c.DstChannel, $"SIP/R1_%")) : false)
                                    ))
                        .OrderByDescending(x => x.Id)
                        .QueryEntityResult(filter)
                        .ProjectTo<ContactDetailDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }
}
