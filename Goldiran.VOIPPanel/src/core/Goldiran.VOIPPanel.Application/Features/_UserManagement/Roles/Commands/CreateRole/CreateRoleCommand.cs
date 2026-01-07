using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.CreateRole;

public class CreateRoleCommand:BaseCreateCommandRequest, IRequest<bool>
{
    public string? Name { get; set; }

    public class Handler : IRequestHandler<CreateRoleCommand, bool>
    {
        private readonly IAppRoleManager _roleManager;

        public Handler(IAppRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = new AppRole()
            {
                Name=request.Name,              
            };

            await _roleManager.CreateAsync(entity);

            return true;
        }
    }
}
