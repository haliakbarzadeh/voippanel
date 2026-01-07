using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.IQueries;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class TempFlatDataJobQueryService : ITempFlatDataJobQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public TempFlatDataJobQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TempFlatDataJobDto> GetLastFlatData(GetTempFlatDataJobLastQuery filter)
    {
        var entity=await _context.TempFlatDataJobs.AsNoTracking()
            .Where(c=>filter.ReportType!=null?c.ReportType==filter.ReportType:true)
            .OrderBy(c=>c.Id).LastOrDefaultAsync();
        return _mapper.Map<TempFlatDataJobDto>(entity);
    }

    

}
