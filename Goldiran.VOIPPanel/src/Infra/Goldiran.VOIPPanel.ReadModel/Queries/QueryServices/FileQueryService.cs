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

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class FileQueryService : IFileQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public FileQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<FileDto> GetFileById(Guid id)
    {
        var entity=await _context.File.Include(c=>c.FileContent).AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id || c.FileOwnerId==id);
        return _mapper.Map<FileDto>(entity);
    }

    public async Task<PaginatedList<FileRowDto>> GetFiles(GetFilesQuery filter)
    {
        var result = await _context.File.AsNoTracking()
                        .Where(c => (filter.FileOwnerId!=null ? c.FileOwnerId == filter.FileOwnerId : true) &&
                                    (filter.FileOwnerTypeId != null ? c.FileOwnerTypeId == filter.FileOwnerTypeId : true) )
                        .OrderByDescending(x => x.Created)
                        .QueryEntityResult(filter)
                        .ProjectTo<FileRowDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

}
