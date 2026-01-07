using AutoMapper;
using Dapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Persistence.AnsweringMachines;

public class AnsweringMachineRepository : IAnsweringMachineRepository
{
    private readonly MySqlConnection _sqlConnection;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private int _retry = 0;

    public AnsweringMachineRepository( MySqlConnection sqlConnection, IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        _configuration = configuration;

    }
    public async Task<bool> Update(AnsweringMachineRequest request)
    {
        _sqlConnection.ConnectionString= _configuration.GetValue<string>("ConnectionStrings:MySQLPersistConnection");
        if (_sqlConnection.State != ConnectionState.Open)
            _sqlConnection.Open();

        string sp = "Change_AM_Status";
       

        var parameters = new DynamicParameters();

        parameters.Add("@idNumber", request.Id, DbType.Int32);
        parameters.Add("@agent", request.Agent, DbType.Int32);
        parameters.Add("@status", request.Status, DbType.Int32);
        parameters.Add("@description", request.Description, DbType.String);
        parameters.Add("@date", request.EditDate, DbType.Date);
        parameters.Add("@time", request.EditDate, DbType.Time);
        parameters.Add("@updatetime", request.UpdateTime, DbType.DateTime);


        var result = await _sqlConnection.QueryAsync<int>(sp, parameters, null, 300, CommandType.StoredProcedure);

        return true;
    }
}
