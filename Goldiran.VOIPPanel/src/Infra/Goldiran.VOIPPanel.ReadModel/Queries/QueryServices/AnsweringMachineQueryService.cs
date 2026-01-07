using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MySqlConnector;
using System.Data;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Voip.Framework.EFCore.Extensions;


namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class AnsweringMachineQueryService : IAnsweringMachineQueryService
{
    private AsteriskReadModelContext _askContext;
    private VOIPPanelReadModelContext _context;
    private readonly MySqlConnection _sqlConnection;
    private readonly IReadModelContext _readModelContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cach;
    private int _retry = 0;

    public AnsweringMachineQueryService(AsteriskReadModelContext askContext, VOIPPanelReadModelContext context, MySqlConnection sqlConnection, IReadModelContext readModelContext, IMapper mapper, IDistributedCache cach)
    {
        _context = context;
        _askContext = askContext;
        _sqlConnection = sqlConnection;
        _readModelContext=readModelContext;
        _mapper = mapper;
        _cach = cach;
    }

    public async Task<PaginatedList<AnsweringMachineDto>> GetAnsweringMachines(GetAnsweringMachinesQuery filter)
    {


            DateTime? fromDate = filter.FromDate!=null? ((DateTime)filter.FromDate).Add(new TimeSpan(0, 0, 0)):null;
            DateTime? toDate = filter.ToDate != null ? ((DateTime)filter.ToDate).Add(new TimeSpan(23, 59, 59)) : null;
        //var result = await _askContext.AnsweringMachines.AsNoTracking()
        //                    .Where(c => 
        //                            (!string.IsNullOrEmpty(filter.Number) ? EF.Functions.Like(c.CallerId, $"%{filter.Number}%") || EF.Functions.Like(c.Mob, $"%{filter.Number}%") || EF.Functions.Like(c.Phone, $"%{filter.Number}%") : true) &&
        //                            (fromDate != null ? c.Date >= fromDate : true) &&
        //                            (toDate != null ? c.Date <= toDate : true)
        //                        )
        //                .OrderByDescending(x => x.Date)
        //                .ProjectTo<AnsweringMachineDto>(_mapper.ConfigurationProvider)
        //                .PaginatedListAsync(filter.PageNumber, filter.PageSize);
        var users = _context.User.AsNoTracking();

        var query = from cust in _askContext.AnsweringMachines
                    orderby cust.Id descending
                    where (fromDate != null ? cust.Date >= fromDate : true) &&
                          (toDate != null ? cust.Date <= toDate : true) &&
                          (filter.AMStatus != null ? cust.Status == filter.AMStatus : true) &&
                          (!string.IsNullOrEmpty(filter.Number) ? EF.Functions.Like(cust.CallerId, $"%{filter.Number}%") || EF.Functions.Like(cust.Mob, $"%{filter.Number}%") || EF.Functions.Like(cust.Phone, $"%{filter.Number}%") : true) 
                        select new { cust };



        var count = query.Count();

        var entities = await query
        .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToListAsync();

        //var results = from cust in entities
        //              join up in users on cust.cust.Agent equals up.Id into userses
        //              from user in userses.DefaultIfEmpty()
        //              orderby cust.cust.Id descending
        //              select new { cust, user };

        var dtos = new List<AnsweringMachineDto>();

        foreach (var entity in entities)
        {
            string username=string.Empty;
            if(entity.cust.Agent!=null)
                username = _context.User.AsNoTracking().FirstOrDefault(c=>c.Id== entity.cust.Agent).PersianFullName;

            var dto = new AnsweringMachineDto()
            {
                Id = entity.cust.Id,
                CallerId = entity.cust.CallerId,
                Agent = entity.cust.Agent,
                Date = entity.cust.Date,
                Queue = entity.cust.Queue,
                Time = entity.cust.Time,
                EditDate = entity.cust.EditDate,
                Flag = entity.cust.Flag,
                Status = entity.cust.Status,
                Description = entity.cust.Description,
                CallerName = entity.cust.CallerName,
                EditTime = entity.cust.EditTime,
                Mob = entity.cust.Mob,
                Phone = entity.cust.Phone,
                UpdateTime = entity.cust.UpdateTime,
                UserName=username
                
            };

            dtos.Add(dto);
        }

        return new PaginatedList<AnsweringMachineDto>(dtos, count, filter.PageNumber, filter.PageSize);

    }


}
