using AutoMapper;
using Dapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class RasadOperationQueryService : IRasadOperationQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly SqlConnection _sqlConnection;

    private readonly IMapper _mapper;
    public RasadOperationQueryService(VOIPPanelReadModelContext context, SqlConnection sqlConnection, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        if (_sqlConnection.State != ConnectionState.Open)
            _sqlConnection.Open();
    }
    public async Task<RasadOperationDto> GetRasadOperation(GetRasadOperationQuery filter)
    {
        var parameters = new
        {

        };

        var query = GetRasadUserOperationQuery(filter);

        var rasadUserOperationDtoResult = await _sqlConnection.QueryAsync<RasadUserOperationDto>(query, parameters, null, 80);

        await _sqlConnection.CloseAsync();
        var rasadUserOperationDtoList=new List<RasadUserOperationDto>();

        if (rasadUserOperationDtoResult.Count()!=0)
        {
            rasadUserOperationDtoList=rasadUserOperationDtoResult.ToList();
        }

        var rasadTeamOperationDtoList = new List<RasadTeamOperationDto>();

        for (int i = 1; i < 10; i++)
        {
            rasadTeamOperationDtoList.Add(new RasadTeamOperationDto() { OperationTypeId=(OperationType)(i), Count=rasadUserOperationDtoList.Where(c=>(int)c.OperationTypeId==i).Count()});
        }

        return new RasadOperationDto() { RasadUserOperationList = rasadUserOperationDtoList, RasadTeamOperationDtoList = rasadTeamOperationDtoList };

    }

    private string GetRasadUserOperationQuery(GetRasadOperationQuery filter)
    {
        string monitoredPositionIds=string.Empty;
        if(!filter.MonitoredPositionIds.IsNullOrEmpty())
            monitoredPositionIds=$"({string.Join(',',filter.MonitoredPositionIds)})";
        //DATEDIFF(minute,CONVERT(DATETIME, CONVERT(CHAR(8), op.StartDate, 112)+ ' ' + CONVERT(CHAR(8), op.StartTime, 108)),GETDATE()) as Duration,
        string query = $"WITH RankedUserPosition AS (SELECT up.PositionId,up.UserId, up.extension, up.isactive,ROW_NUMBER() OVER (PARTITION BY up.userid,up.positionId ORDER BY up.positionid DESC) AS rn  FROM  UserPosition as up where up.isactive=1)" +
            $" select op.UserId,op.PositionId,u.PersianFullName as UserFullName, op.OperationTypeId, op.StartDate, op.StartTime, op.EndDate, op.EndTime, u.PersianFullName, up.extension " +
            $" from operation as op inner join [User] as u on op.UserId=u.Id inner join RankedUserPosition as up on (u.id=up.UserId and op.PositionId=up.PositionId) inner join Position as p on up.PositionId=p.id where  u.isActive=1 and up.rn=1 and up.isactive=1 and op.IsCurrentStatus=1 and (p.ContactedParentPositionId like '%''{filter.PositionId}''%' ";

        if (!string.IsNullOrEmpty(monitoredPositionIds))
        {
            query += $" or p.Id in {monitoredPositionIds}) ";
        }
        else
        {
            query += " ) ";
        }

        if (filter.OperationTypeId != null)
        {
            query += $" and op.OperationTypeId={Convert.ToInt16(filter.OperationTypeId)} ";
        }

        query += $" order by op.UserId asc";
        return query;
    }
}
