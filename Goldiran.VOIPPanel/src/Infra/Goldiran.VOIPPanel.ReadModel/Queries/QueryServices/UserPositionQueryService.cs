using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.QueryHandler.Queries.QueryServices;

public class UserPositionQueryService : IUserPositionQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public UserPositionQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<UserPositionDto> GetUserPositionById(long id)
    {
        var entity = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return _mapper.Map<UserPositionDto>(entity);
    }

    public async Task<PaginatedList<UserPositionDto>> GetUserPositions(GetUserPositionsQuery filter)
    {
        var result = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                        .Where(c => (filter.UserId != null ? c.UserId == filter.UserId : true) &&
                                    (filter.PositionId != null ? c.PositionId == filter.PositionId : true) &&
                                    (filter.IsActivePosition != null ? c.IsActive == filter.IsActivePosition : true) &&
                                    (filter.Search != null ? (EF.Functions.Like(c.User.PersianFullName, $"%{filter.Search}%") || EF.Functions.Like(c.Extension, $"{filter.Search}%")) : true) &&
                                    (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId)) : true))
                        .OrderByDescending(x => x.Id)
                        //.QueryEntityResult(filter)
                        .ProjectTo<UserPositionDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<PaginatedList<UserPositionDto>> GetUserPositions(GetOperationUsersQuery filter)
    {
        var result = await _context.UserPosition.Include(c => c.Position).Include(c => c.User).AsNoTracking()
                        .Where(c => (c.User.IsActive) &&
                                    (filter.IsActivePosition != null ? c.IsActive == filter.IsActivePosition : true) &&
                                    (!filter.PositionIds.IsNullOrEmpty() ? filter.PositionIds.Contains(c.PositionId) : true) &&
                                    (filter.Search != null ? (EF.Functions.Like(c.User.PersianFullName, $"%{filter.Search}%") || EF.Functions.Like(c.Extension, $"{filter.Search}%")) : true) &&
                                    (filter.IsRestricted && !filter.HasContentAccess ? (EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{filter.PositionId}'%") || filter.MonitoredPositionIds.Contains(c.PositionId) || (c.UserId == filter.UserId && c.PositionId == filter.PositionId)) : true))
                        .OrderByDescending(x => x.Id)
                        //.QueryEntityResult(filter)
                        .ProjectTo<UserPositionDto>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync(filter.PageNumber, filter.PageSize, filter.DisablePaging);

        return result;
    }


    //public async Task<List<UserPosition>> GetUserPositionsList(GetUserPositionsListQuery filter)
    //{
    //    var result = await _context.UserPositions.Include(c => c.Position).AsNoTracking()
    //                                            //.Where(c => (!filter.Title.IsNullOrEmpty() ? c.ContactedParentPositionName.Contains(filter.Title) : true))
    //                                            //.ProjectTo<UserPositionDto>(_mapper.ConfigurationProvider)
    //                    .ToListAsync();

    //    return result;
    //}
}
