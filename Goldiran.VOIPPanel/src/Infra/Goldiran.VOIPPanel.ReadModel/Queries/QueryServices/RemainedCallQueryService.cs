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
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Goldiran.VOIPPanel.ReadModel.Enums;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class RemainedCallQueryService : IRemainedCallQueryService
{
    private readonly MySqlConnection _sqlConnection;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public RemainedCallQueryService(VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        _configuration = configuration;

    }

    public async Task<RemainedCallDto> GetRemainedCall(GetRemainedCallQuery filter)
    {
        var dto=new RemainedCallDto();
        string queryCount=string.Empty;

        if (_sqlConnection.State != ConnectionState.Open)
        {
            _sqlConnection.ConnectionString= _configuration.GetValue<string>("ConnectionStrings:MySQLMarkettingConnection");
            _sqlConnection.Open();
        }
        if (filter.RemainedCallType == RemainedCallType.Marketting)
        {
            filter.IsTotall= true;
            queryCount = GetMarkettingQueryCount(filter);
            try
            {

                dto.TotalCount = await _sqlConnection.QueryFirstAsync<int>(queryCount, null, null, 80);
            }
            catch (Exception)
            {
            }

            filter.IsTotall =false;
            filter.IsTehran=true;
            queryCount = GetMarkettingQueryCount(filter);
            try
            {

                dto.TehranRemainedCount = await _sqlConnection.QueryFirstAsync<int>(queryCount, null, null, 80);
            }
            catch (Exception)
            {
            }

            filter.IsTehran = false;
            queryCount = GetMarkettingQueryCount(filter);
            try
            {

                dto.ProvinceRemainedCount = await _sqlConnection.QueryFirstAsync<int>(queryCount, null, null, 80);
            }
            catch (Exception)
            {
            }

            if (dto.TehranRemainedCount != null && dto.ProvinceRemainedCount != null)
            {
                dto.TotalRemainedCount=dto.TehranRemainedCount+dto.ProvinceRemainedCount;
            }
        }
        else
        {

            filter.IsTotall = true;
            queryCount = GetTamdidQueryCount(filter);
            try
            {

                dto.TotalCount = await _sqlConnection.QueryFirstAsync<int>(queryCount, null, null, 80);
            }
            catch (Exception)
            {
            }

            filter.IsTotall = false;
            queryCount = GetTamdidQueryCount(filter);
            try
            {

                dto.TotalRemainedCount = await _sqlConnection.QueryFirstAsync<int>(queryCount, null, null, 80);
            }
            catch (Exception)
            {
            }

        }

                await _sqlConnection.CloseAsync();

        return dto;
    }


    private string GetMarkettingQueryCount(GetRemainedCallQuery filter)
    {
        string query = $"select count(id) from asteriskcdrdb.testmarketingAD as o where date>='{filter.FromDate.Year}-{filter.FromDate.Month.ToString("00")}-{filter.FromDate.Day.ToString("00")}' and  date<='{filter.ToDate.Year}-{filter.ToDate.Month.ToString("00")}-{filter.ToDate.Day.ToString("00")}' ";

        if (filter.IsTehran != null && (bool)filter.IsTehran)
        {
            query += "and  o.cityid=1 ";
        }
        if (filter.IsTehran != null && !(bool)filter.IsTehran)
        {
            query += "and  o.cityid<>1 ";
        }

        if (!filter.IsTotall)
        {
            query += "and  o.flag=0 ";
        }

        return query;
    }

    private string GetTamdidQueryCount(GetRemainedCallQuery filter)
    {
        string query = $"select count(id) from asteriskcdrdb.testTamdidAD as o where date>='{filter.FromDate.Year}-{filter.FromDate.Month.ToString("00")}-{filter.FromDate.Day.ToString("00")}' and  date<='{filter.ToDate.Year}-{filter.ToDate.Month.ToString("00")}-{filter.ToDate.Day.ToString("00")}' ";

        if (!filter.IsTotall)
        {
            query += "and  o.flag=0 ";
        }

        return query;
    }




}
