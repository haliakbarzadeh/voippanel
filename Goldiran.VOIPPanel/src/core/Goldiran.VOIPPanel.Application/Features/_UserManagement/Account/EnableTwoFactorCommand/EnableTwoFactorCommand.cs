using Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using ValidationException = Saramad.Core.ApplicationService.Common.Exceptions.ValidationException;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.EnableTwoFactorCommand;

public class EnableTwoFactorCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public int UserId { get; set; }
    public string Code { get; set; }
}

public class Handler : IRequestHandler<EnableTwoFactorCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;

    public Handler(IApplicationDbContext db, IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions)
    {
        _db = db;
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
    }

    public async Task<bool> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        if (user.TwoFactorEnabled)
            throw new InvalidOperationException("این قابلیت قبلا برای کاربر فعال شده است");

        var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

        if (!is2faTokenValid)
            throw new InvalidOperationException("کد معتبر نمی باشد");

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);

        var result=await _userManager.UpdateAsync(user);
        await _db.SaveChangesAsync(cancellationToken);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
        }
        return true;
        
    }
}


