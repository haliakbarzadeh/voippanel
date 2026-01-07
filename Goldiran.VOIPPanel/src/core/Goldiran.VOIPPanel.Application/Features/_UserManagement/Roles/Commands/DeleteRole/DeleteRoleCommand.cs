using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(long Id) : IRequest<bool>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand,bool>
{
    private readonly IAppRoleManager _roleManager;

    public DeleteRoleCommandHandler( IAppRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _roleManager.Roles
            .SingleOrDefaultAsync(c=>c.Id==request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(IAppRoleManager), request.Id);
        }

        await _roleManager.DeleteAsync(entity);

        return true;
    }
}
