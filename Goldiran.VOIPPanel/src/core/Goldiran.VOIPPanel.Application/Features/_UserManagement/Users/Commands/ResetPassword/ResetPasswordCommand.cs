using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ResetPassword;

public class ResetPasswordCommand : BaseUpdateCommandRequest, IRequest<bool>
    {
    public string UserName { get; set; }
    public string NewPassword { get; set; }


    public class Handler : IRequestHandler<ResetPasswordCommand, bool>
    {
            private IMapper _mapper;
            private IAppUserManager _userManager;
            public Handler(IMapper mapper, IAppUserManager userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                    return false;

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
                if (result.Succeeded)
                {
                    //user.IsTemperory = false;
                    await _userManager.UpdateAsync(user);
                    return true;
                }
                else
                {
                    throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
                }

            }
    }
    }

