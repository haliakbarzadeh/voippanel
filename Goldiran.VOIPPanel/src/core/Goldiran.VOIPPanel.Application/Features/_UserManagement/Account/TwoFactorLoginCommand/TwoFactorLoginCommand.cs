using Framework.Domain;
using MediatR;
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

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.TwoFactorLoginCommand;

public class TwoFactorLoginCommand : BaseCreateCommandRequest, IRequest<LoginResponse>
{
    public string Code { get; private set; }
    public bool RememberMachine { get; private set; }
    public string IP { get; private set; }
    public string UserAgent { get; private set; }
}

public class Handler : IRequestHandler<TwoFactorLoginCommand, LoginResponse>
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

    public async Task<LoginResponse> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        var signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(request.Code, false, request.RememberMachine);
        if (!signInResult.Succeeded)
        {
            throw new InvalidOperationException("کد معتبر نمی باشد.");
        }
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("اکانت شما غیرفعال شده‌است.");
        }

        if (_siteOptions.Value.EnableEmailConfirmation &&
            !await _userManager.IsEmailConfirmedAsync(user))
        {
            throw new InvalidOperationException("لطفا به پست الکترونیک خود مراجعه کرده و ایمیل خود را تائید کنید!");
        }


        if (user.IsTemperory)
        {
            var token = await _iTokenService.GenerateAccessToken(user);
            return new LoginResponse()
            {
                IsTemporary = false,
                UserId= user.Id,
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }
        else 
        {
            return new LoginResponse()
            {
                IsTemporary = true,
                UserId = user.Id
            };
        }

    }
}


