using AutoMapper;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Queries;
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleQueryService _roleManager;
    private readonly IMapper _mapper;

    public GetRoleByIdQueryHandler(IRoleQueryService roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _roleManager.GetRoleById(request.Id);

    }
}
