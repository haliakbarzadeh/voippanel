using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Dapper;
using Goldiran.VOIPPanel.Domain.Common.Enums;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class SecureCallQueryService : ISecureCallQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly SqlConnection _sqlConnection;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public SecureCallQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, SqlConnection sqlConnection, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _askContext = askContext;
        _mapper = mapper;
        _sqlConnection = sqlConnection;
        _configuration = configuration;
    }


    public async Task<PaginatedList<SecureCallDto>> GetSecureCalls(GetSecureCallsQuery filter)
    {
        bool flag=false;
        List<string> techNumbers = new List<string>();

        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;

        var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(c => c.Id == filter.UserId);

        if (user != null && user.UserType==UserType.Agent ) 
        {
            flag=true;

            if (!string.IsNullOrEmpty(user.PersonalCode))
            {
                techNumbers = await GetTechNumbers(Convert.ToInt32(user.PersonalCode));

            }

            if (techNumbers.IsNullOrEmpty())
            {
                return new PaginatedList<SecureCallDto>(null, 0, 1, 10);
            }
        }

        var result = await _askContext.SecureCalls.AsNoTracking()
                            .Where(c => (filter.Type != null ?((filter.Type== SecureReportType.TechToCust || filter.Type == SecureReportType.CustToTech) ?c.Type==filter.Type:c.TechNumber=="CIC IVR" ): true) &&
                                    (!string.IsNullOrEmpty(filter.Number) ?c.TechNumber==filter.Number || c.CustomerNumber == filter.Number : true) &&
                                    (fromDate != null ? c.Date >= fromDate : true) &&
                                    (toDate != null ? c.Date <= toDate : true) &&
                                    (flag?techNumbers.Contains(c.TechNumber):true)
                                )
                        .OrderByDescending(x => x.Date)
                        .ProjectTo<SecureCallDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;

    }

    public async Task<GeneralSecureCallDto> GetGeneralSecureCall(GetGeneralSecureCallQuery filter)
    {
        DateTime? fromDate = filter.FromDate != null ? filter.FromDate.Value.Date.Add(new TimeSpan(0, 0, 0)) : null;
        DateTime? toDate = filter.ToDate != null ? filter.ToDate.Value.Date.Add(new TimeSpan(23, 59, 59)) : null;


        var query = _askContext.SecureCalls.AsNoTracking()
                            .Where(c =>(!string.IsNullOrEmpty(filter.Number) ? c.TechNumber == filter.Number || c.CustomerNumber == filter.Number : true) &&
                                    (fromDate != null ? c.Date >= fromDate : true) &&
                                    (toDate != null ? c.Date <= toDate : true)
                                );

        var generalSecureCallDto=new GeneralSecureCallDto();

        generalSecureCallDto.TechToCust=query.Where(c=>c.Type== SecureReportType.TechToCust).Count();
        generalSecureCallDto.SucTechToCust = query.Where(c => c.Type == SecureReportType.TechToCust && c.TechCallStatus== "Success").Count();

        generalSecureCallDto.CustToTech = query.Where(c => c.Type == SecureReportType.CustToTech ).Count();
        generalSecureCallDto.SucCustToTech = query.Where(c => c.Type == SecureReportType.CustToTech && c.CustomerCallStatus == "ANSWER").Count();

        generalSecureCallDto.CustToInfo = query.Where(c => c.TechNumber == "CIC IVR").Count();
        generalSecureCallDto.SucCustToInfo = query.Where(c => c.TechNumber == "CIC IVR" && c.CustomerCallStatus == "ANSWER").Count();


        return generalSecureCallDto;
    }

    public async Task<List<string>> GetTechNumbers(long personelCode)
    {

        if (_sqlConnection.State != ConnectionState.Open)
        {
            _sqlConnection.ConnectionString = _configuration.GetValue<string>("ConnectionStrings:GSReadModelConnection");
            _sqlConnection.Open();
        }

        var parameters = new
        {
        };

        var query = $"SELECT [Mobile] FROM [GoldServiceMain].[dbo].[VW_VoipTechnicians] where  ASCCode={personelCode}";

        var result = await _sqlConnection.QueryAsync<string>(query, parameters, null, 80);


        return result.ToList();
    }
}

public class TechNumberDto 
{
    public string TechNumber { get; set; } = string.Empty;
}

