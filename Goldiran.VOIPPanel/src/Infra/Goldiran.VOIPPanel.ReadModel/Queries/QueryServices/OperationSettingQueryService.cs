using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class OperationSettingQueryService : IOperationSettingQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public OperationSettingQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<OperationSettingDto> GetOperationSettingById(int id)
    {
        var entity=await _context.OperationSetting.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        return _mapper.Map<OperationSettingDto>(entity);
    }

    public async Task<PaginatedList<OperationSettingDto>> GetOperationSettings(GetOperationSettingsQuery filter)
    {

        var result = await _context.OperationSetting.AsNoTracking()
                        .Where(c =>(filter.OperationTypeId!=null ? c.OperationTypeId == filter.OperationTypeId: true) &&
                                    (filter.IsActive != null ? c.IsActive == filter.IsActive : true) &&
                                    (filter.ShowToUser != null ? c.ShowToUser == filter.ShowToUser : true))
                        .OrderBy(x => x.Order)
                        .QueryEntityResult(filter)
                        .ProjectTo<OperationSettingDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }


}
