using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Queries;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PaginatedList<RoleDto>>
{
    private readonly IRoleQueryService _roleManager;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(IRoleQueryService roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleManager.GetRoles(request);
    }
}

