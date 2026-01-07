using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public List<long> RoleIdList { get; set; }

    public class Handler : IRequestHandler<ChangeUserRoleCommand, bool>
    {
        private IMapper _mapper;
        private IAppUserManager _userManager;
        public Handler(IMapper mapper, IAppUserManager userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManager.AddOrUpdateUserRolesAsync(request.Id, request.RoleIdList);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            return true;
        }

    }
}

