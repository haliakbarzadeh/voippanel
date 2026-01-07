using ValidationException=Voip.Framework.Common.Exceptions.ValidationException;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Common.Services.IdentityService;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Voip.Framework.Common.AppSettings;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.ChangePasswordCommand;

public class ChangePasswordCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long UserId { get;  set; }
    public string CurrentPassword { get;  set; }
    public string NewPassword { get;  set; }
    public string ReNewPassword { get;  set; }
}

public class Handler : IRequestHandler<ChangePasswordCommand, bool>
{
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;

    public Handler( IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions)
    {
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        var result =await _userManager.ChangePasswordAsync(user, request.CurrentPassword,request.NewPassword);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
        }

        return true;
        
    }
}


