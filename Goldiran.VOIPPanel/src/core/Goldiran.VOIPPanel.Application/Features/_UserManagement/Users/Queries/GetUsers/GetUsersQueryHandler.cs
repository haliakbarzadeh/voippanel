using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Cube.UserManagementService.Domain.AggregatesModel.UserPositions.Contracts;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Queries.GetUser;


public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
{
    private readonly IUserQueryService _userManager;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IUserQueryService userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.GetUsers(request);
    }
}
