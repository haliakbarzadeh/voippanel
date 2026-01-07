using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ActivateUser;

public class ActivateUserCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public bool IsActive { get; set; }


    public class Handler : IRequestHandler<ActivateUserCommand, bool>
    {
        private IMapper _mapper;
        private IAppUserManager _userManager;
        public Handler(IMapper mapper, IAppUserManager userManager)
        {

            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                throw new NotFoundException("کاربر مورد نظر یافت نشد");
            user.IsActive = request.IsActive;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            return true;
        }

    }
}

