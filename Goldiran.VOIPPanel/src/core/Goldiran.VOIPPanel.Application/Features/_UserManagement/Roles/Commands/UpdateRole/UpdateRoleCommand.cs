using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.UpdateRole;

public class UpdateRoleCommand:BaseUpdateCommandRequest, IRequest<bool>
{
    public long? Id { get; init; }
    public string? Name { get; set; }

    public class Handler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly IAppRoleManager _roleManager;

        public Handler( IAppRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _roleManager.Roles
                .FirstAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IAppRoleManager), request.Id);
            }
            entity.Name=request.Name;

            await _roleManager.UpdateAsync(entity);

            return true;
        }
    }
}
