using AutoMapper;
using AutoMapper.QueryableExtensions;
using Goldiran.Framework.Domain.Extensions;
using Goldiran.Framework.Domain.Models.CQRS;
using Goldiran.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.Common.Dtos;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Entities.Identity;
using Goldiran.VOIPPanel.ReadModel.Queries.Dtos;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.Queries.QueryFilters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Queries.QueryServices;

public class TokenQueryService : ITokenQueryService
{
    private VOIPPanelReadModelContext _context;
    private readonly IMapper _mapper;
    public TokenQueryService(VOIPPanelReadModelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Token> GetTokenById(long id)
    {
        return await _context.Tokens.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
    }

    public async Task<List<Token>> GetTokensList(GetTokensQuery filter)
    {
        var result = await _context.Tokens.AsNoTracking()
        .Where(c => (filter.UserId!=null? c.UserId == Convert.ToInt64(filter.UserId) :true) && (filter.TokenValue != null ? c.TokenValue == filter.TokenValue:true) && (filter.RefreshToken!=null?c.RefreshToken == filter.RefreshToken:true) && DateTime.Now < c.RefreshTokenExpiryTime)
                        .ToListAsync();

        return result;
    }
}
