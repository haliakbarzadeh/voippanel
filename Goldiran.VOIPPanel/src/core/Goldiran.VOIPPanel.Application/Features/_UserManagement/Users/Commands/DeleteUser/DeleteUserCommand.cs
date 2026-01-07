using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.DeleteUser;

public class DeleteUserCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,bool>
{
    private IAppUserManager _userManager;
    private IMapper _mapper;
    public DeleteUserCommandHandler(IAppUserManager userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        await _userManager.DeleteAsync(user);

        return true;
    }

}
