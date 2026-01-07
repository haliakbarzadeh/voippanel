using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Queries;
public class GetRolePermission : IRequest<List<PermissionNodes>>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<GetRolePermission, List<PermissionNodes>>
    {
        private readonly IAppRoleManager _roleManager;
        private readonly IMapper _mapper;

        public Handler(IAppRoleManager roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<PermissionNodes>> Handle(GetRolePermission request, CancellationToken cancellationToken)
        {

            var result = await _roleManager.GetRolePermission(request.Id);

            return result;
        }



    }
}
