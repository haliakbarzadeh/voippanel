using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.ChangeAccesses;

public class ChangeAccessesCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public List<string> Permissions { get; set; }

    public class Handler : IRequestHandler<ChangeAccessesCommand, bool>
    {
        private IMapper _mapper;
        private IAppRoleManager _roleManager;
        public Handler(IMapper mapper, IAppRoleManager roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(ChangeAccessesCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleManager.ChangeAccessesAsync(request.Id, request.Permissions);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            return true;
        }
    }
}

