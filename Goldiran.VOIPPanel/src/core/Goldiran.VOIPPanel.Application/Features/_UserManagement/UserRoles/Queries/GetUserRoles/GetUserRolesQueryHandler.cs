using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;

namespace Goldiran.VOIPPanel.Application.Features._UserManagement.UserRoles.Queries.GetUserRoles;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, PaginatedList<UserRoleDto>>
{
    private readonly IUserQueryService _userManager;
    private readonly IMapper _mapper;

    public GetUserRolesQueryHandler(IUserQueryService userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.GetUserRoles(request);
    }
}

