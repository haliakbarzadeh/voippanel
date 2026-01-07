using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class OperationQueryService : IOperationQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly SqlConnection _sqlConnection;

    private readonly IMapper _mapper;
    public OperationQueryService(VOIPPanelReadModelContext context, SqlConnection sqlConnection, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        if (_sqlConnection.State != ConnectionState.Open)
            _sqlConnection.Open();
    }
    public async Task<OperationDto> GetOperationById(long id)
    {
        var entity = await _context.Operation.Include(c => c.User).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return _mapper.Map<OperationDto>(entity);
    }
    public async Task<PaginatedList<OperationDto>> GetOperations(GetOperationsQuery filter)
    {
        List<long> UserIds = new List<long>();
        List<long> PositionIds = new List<long>();

        if (!string.IsNullOrEmpty(filter.UserIds))
            UserIds = filter.UserIds.Split(',').Select(e => Convert.ToInt64(e)).ToList();
        //if (!string.IsNullOrEmpty(filter.PositionIds))
        //    PositionIds = filter.PositionIds.Split(',').Select(e => Convert.ToInt64(e)).ToList();
        SetDuration(filter);
        //        var result = await _context.Operation.Include(c => c.User).ThenInclude(c=>c.UserPositions).Include(c => c.ManagerUser).AsNoTracking()
        //                        .Where(c => (filter.IsCurrentStatus != null ? c.IsCurrentStatus == filter.IsCurrentStatus : true) &&
        //                                    (filter.OperationTypeId != null ? c.OperationTypeId == filter.OperationTypeId : true) &&
        //                                    (filter.User != null ? c.UserId == filter.User : true) &&
        //                                    (!filter.UserIds.IsNullOrEmpty() ? filter.UserIds.Contains(c.UserId) : true) &&
        //                                    (!filter.PositionIds.IsNullOrEmpty() ? filter.PositionIds.Contains(c.PositionId) : true) &&
        //                                    (filter.FromDate != null ? c.StartDate >= ((DateTime)filter.FromDate).Date : true) &&
        //                                    (filter.ToDate != null ? (c.StartDate <= ((DateTime)filter.ToDate).Date) || (((DateTime)filter.ToDate).Date == DateTime.Now.Date && c.IsCurrentStatus) : true) &&
        //                                    (!filter.HasContentAccess ? (c.UserId ==filter.UserId && c.PositionId==c.PositionId) && filter.PositionCildIds.Contains(c.PositionId) : true) 
        //)
        //                        .OrderByDescending(x => x.Created)
        //                        .QueryEntityResult(filter)
        //                        .ProjectTo<OperationDto>(_mapper.ConfigurationProvider)
        //                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);
        //var t = ((DateTime)filter.FromDate).Date;
        var result = await _context.Operation.Include(c => c.User).ThenInclude(c => c.UserPositions).Include(c => c.ManagerUser).AsNoTracking()
                        .Where(c => (filter.IsCurrentStatus != null ? c.IsCurrentStatus == filter.IsCurrentStatus : true) &&
                                    (filter.OperationTypeId != null ? c.OperationTypeId == filter.OperationTypeId : true) &&
                                    (filter.User != null ? c.UserId == filter.User : true) &&
                                    (!UserIds.IsNullOrEmpty() ? UserIds.Contains(c.UserId) : true) &&
                                    (!filter.PositionIds.IsNullOrEmpty() ? filter.PositionIds.Contains(c.PositionId) : true) &&
                                    (filter.FromDate != null ? c.StartDate >= ((DateTime)filter.FromDate).Date : true) &&
                                    (filter.ToDate != null ? (c.StartDate <= ((DateTime)filter.ToDate).Date) || (((DateTime)filter.ToDate).Date == DateTime.Now.Date && c.IsCurrentStatus) : true) &&
                                    (!filter.HasContentAccess ? (c.UserId == filter.UserId && c.PositionId == c.PositionId) || filter.PositionCildIds.Contains(c.PositionId) || filter.MonitoredPositionIds.Contains(c.PositionId) : true)
                                )
                        .OrderByDescending(x => x.Created)
                        .QueryEntityResult(filter)
                        .ProjectTo<OperationDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    private void SetDuration(GetOperationsQuery filter)
    {
        List<DateTime> result = new List<DateTime>();

        if(filter.OperationReportType!=null)
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

    public async Task<OperationDto> GetOperation(GetOperationQuery filter)
    {
        var entity = await _context.Operation.Include(c => c.User).AsNoTracking()
                        .Where(c => (filter.IsCurrentStatus != null ? c.IsCurrentStatus == filter.IsCurrentStatus : true) &&
                                    (filter.OperationTypeId != null ? c.OperationTypeId == filter.OperationTypeId : true) &&
                                    (filter.User != null ? c.UserId == filter.User : true) &&
                                    (filter.Position != null ? c.PositionId == filter.Position : true) &&
                                    (!filter.UserIds.IsNullOrEmpty() ? filter.UserIds.Contains(c.UserId) : true) &&
                                    (filter.FromDate != null ? c.StartDate >= ((DateTime)filter.FromDate).Date : true) &&
                                    (filter.ToDate != null ? (c.StartDate <= ((DateTime)filter.ToDate).Date) || (((DateTime)filter.ToDate).Date == DateTime.Now.Date && c.IsCurrentStatus) : true))
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync();

        if (entity != null)
            return _mapper.Map<OperationDto>(entity);
        else
            return null;
    }

    //public async Task<PaginatedList<AggregateOperationDto>> GetGroupOperations(GetGroupOperationsQuery filter)
    //{
    //    var result = await _context.Operation.Include(c => c.User).AsNoTracking()
    //                    .Where(c => (filter.IsCurrentStatus != null ? c.IsCurrentStatus == filter.IsCurrentStatus : true) &&
    //                                (filter.OperationTypeId != null ? c.OperationTypeId == filter.OperationTypeId : true) &&
    //                                (filter.User != null ? c.UserId == filter.User : true) &&
    //                                (!filter.UserIds.IsNullOrEmpty() ? filter.UserIds.Contains(c.UserId) : true) &&
    //                                (filter.FromDate != null ? c.StartDate >= ((DateTime)filter.FromDate).Date : true) &&
    //                                (filter.ToDate != null ? (c.StartDate <= ((DateTime)filter.ToDate).Date) || (((DateTime)filter.ToDate).Date == DateTime.Now.Date && c.IsCurrentStatus) : true))
    //                    .OrderByDescending(x => x.Created)
    //                    .ProjectTo<OperationDto>(_mapper.ConfigurationProvider)
    //                    .GroupBy(c => new { c.OperationTypeId, c.UserId, c.UserFullName })
    //                    .Select(c => new AggregateOperationDto() { UserId = c.Key.UserId, UserFullName = c.Key.UserFullName, OperationTypeId = c.Key.OperationTypeId, Duration = c.Sum(c => c.Duration ) })
    //                    .PaginatedListAsync(filter.PageNumber, filter.PageSize);

    //    return result;
    //}

    public async Task<PaginatedList<AggregateOperationDto>> GetGroupOperations(GetGroupOperationsQuery filter)
    {

        var parameters = new
        {
            //NationalCode = !string.IsNullOrEmpty(query.NationalCode) ? query.NationalCode.Trim() : string.Empty,
            //pageNumber = query.PageNumber,
            //pageSize = query.PageSize
        };
        var queryCount = GetQueryCount(filter);
        int count = 0;
        try
        {

            count = await _sqlConnection.QueryFirstAsync<int>(queryCount, parameters, null, 80);
        }
        catch (Exception)
        {
        }

        var queryResult = GetQuery(filter);
        //_logger.LogInformation($"test suborder  {queryResult} {DateTime.Now} phase part2_3");
        var result = await _sqlConnection.QueryAsync<AggregateOperationDto>(queryResult, parameters, null, 80);

        await _sqlConnection.CloseAsync();
        if (count > 0)
            return new PaginatedList<AggregateOperationDto>(result.ToList(), count, filter.PageNumber, filter.PageSize);
        else
            return new PaginatedList<AggregateOperationDto>(new List<AggregateOperationDto>(), count, filter.PageNumber, filter.PageSize);
    }
    private string GetQueryCount(GetGroupOperationsQuery filter)
    {
        //string query = $"select count(o.userId) from operation as o where 1=1 ";
        string query = $"select count(o.userId) from operation as o where 1=1  ";

        if (filter.ToDate != null)
        {
            query += "and  o.OperationTypeId<>9 ";
        }
        if (filter.IsCurrentStatus != null)
        {
            query += $" and o.IsCurrentStatus={Convert.ToInt16(filter.IsCurrentStatus)} ";
        }
        if (filter.OperationTypeId != null)
        {
            query += $" and o.OperationTypeId={Convert.ToInt16(filter.OperationTypeId)} ";
        }
        if (filter.User != null)
        {
            query += $" and o.UserId={filter.User} ";
        }
        if (filter.Position != null)
        {
            query += $" and o.PositionId={filter.Position} ";
        }
        if (!filter.UserIds.IsNullOrEmpty())
        {
            query += $" and o.UserId in ({string.Join(',', filter.UserIds)}) ";
        }
        if (filter.FromDate != null)
        {
            query += $" and o.StartDate >='{((DateTime)filter.FromDate).Date}' ";
        }
        if (filter.ToDate != null)
        {
            query += $" and (o.StartDate <='{((DateTime)filter.ToDate).Date}' or ('{((DateTime)filter.ToDate).Date}'='{DateTime.Now.Date}' and o.IsCurrentStatus=1)) ";
        }

        query += $" group by o.UserId,o.PositionId,o.OperationTypeId";

        return query;
    }

    private string GetQuery(GetGroupOperationsQuery filter)
    {
        string query = $"select (select os.duration from OperationSetting as os where os.OperationTypeId=t.OperationTypeId) as MojazDuration,(select os.HitLimit from OperationSetting as os where os.OperationTypeId=t.OperationTypeId) as MojazCount, t.UserId,t.OperationTypeId,t.PersianFullName,t.count as Count,t.Duration from (select o.UserId,o.PositionId, o.OperationTypeId,u.PersianFullName,count(o.UserId) as count,";

        //if (filter.FromDate != null && ((DateTime)filter.FromDate).Date == DateTime.Now.Date)
        //    query += $" (select sum( DATEDIFF(minute, CONVERT(DATETIME, CONVERT(CHAR(8), CAST(GETDATE() AS DATE), 112) " +
        //                  $"+ ' ' + CONVERT(CHAR(8), '08:30:00', 108)), isnull(CONVERT(DATETIME, CONVERT(CHAR(8), oo.EndDate, 112) " +
        //                  $"+ ' ' + CONVERT(CHAR(8), oo.EndTime, 108)), getdate()))) ";
        //else
        query += $" (select sum( DATEDIFF(minute, CONVERT(DATETIME, CONVERT(CHAR(8), oo.StartDate, 112) " +
          $"+ ' ' + CONVERT(CHAR(8), oo.StartTime, 108)), isnull(CONVERT(DATETIME, CONVERT(CHAR(8), oo.EndDate, 112) " +
          $"+ ' ' + CONVERT(CHAR(8), oo.EndTime, 108)), getdate()))) ";

        query += $"from operation as oo where o.UserId = oo.UserId and oo.OperationTypeId = o.OperationTypeId   ";
        if (filter.IsCurrentStatus != null)
        {
            query += $" and oo.IsCurrentStatus={Convert.ToInt16(filter.IsCurrentStatus)} ";
        }
        if (filter.OperationTypeId != null)
        {
            query += $" and oo.OperationTypeId={Convert.ToInt16(filter.OperationTypeId)} ";
        }
        if (filter.User != null)
        {
            query += $" and oo.UserId={filter.User} ";
        }
        if (filter.Position != null)
        {
            query += $" and oo.PositionId={filter.Position} ";
        }
        if (!filter.UserIds.IsNullOrEmpty())
        {
            query += $" and oo.UserId in ({string.Join(',', filter.UserIds)}) ";
        }
        if (filter.FromDate != null)
        {
            query += $" and oo.StartDate >='{((DateTime)filter.FromDate).Date}' ";
        }
        if (filter.ToDate != null)
        {
            query += $" and (oo.StartDate <='{((DateTime)filter.ToDate).Date}' or ('{((DateTime)filter.ToDate).Date}'='{DateTime.Now.Date}' and oo.IsCurrentStatus=1)) ";
        }

        query +=$" ) as Duration from Operation as o inner join [User] as u on o.UserId = u.Id where 1=1  ";
        if (filter.ToDate != null)
        {
            query += "and  o.OperationTypeId<>9 ";
        }
        if (filter.IsCurrentStatus != null)
        {
            query += $" and o.IsCurrentStatus={Convert.ToInt16(filter.IsCurrentStatus)} ";
        }
        if (filter.OperationTypeId != null)
        {
            query += $" and o.OperationTypeId={Convert.ToInt16(filter.OperationTypeId)} ";
        }
        if (filter.User != null)
        {
            query += $" and o.UserId={filter.User} ";
        }
        if (filter.Position != null)
        {
            query += $" and o.PositionId={filter.Position} ";
        }
        if (!filter.UserIds.IsNullOrEmpty())
        {
            query += $" and o.UserId in ({string.Join(',', filter.UserIds)}) ";
        }
        if (filter.FromDate != null)
        {
            query += $" and o.StartDate >='{((DateTime)filter.FromDate).Date}' ";
        }
        if (filter.ToDate != null)
        {
            query += $" and (o.StartDate <='{((DateTime)filter.ToDate).Date}' or ('{((DateTime)filter.ToDate).Date}'='{DateTime.Now.Date}' and o.IsCurrentStatus=1)) ";
        }

        query += $" group by o.UserId,o.positionId,o.OperationTypeId,u.PersianFullName) as t order by t.UserId offset {(filter.PageNumber - 1) * filter.PageSize} rows fetch next {filter.PageSize} rows only";
        return query;
    }
}
