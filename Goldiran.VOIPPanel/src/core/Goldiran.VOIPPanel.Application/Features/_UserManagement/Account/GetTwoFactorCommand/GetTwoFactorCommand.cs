using Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features._Idenetity._Models;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using ValidationException = Saramad.Core.ApplicationService.Common.Exceptions.ValidationException;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.GetTwoFactorCommand;

public class GetTwoFactorCommand : BaseCreateCommandRequest, IRequest<GetTwoFactorResponse>
{
    public int UserId { get; set; }
}

public class Handler : IRequestHandler<GetTwoFactorCommand, GetTwoFactorResponse>
{
    private readonly IApplicationDbContext _db;
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;
    private readonly UrlEncoder _urlEncoder;

    public Handler(IApplicationDbContext db, IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions)
    {
        _db = db;
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
    }

    public async Task<GetTwoFactorResponse> Handle(GetTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        await _userManager.ResetAuthenticatorKeyAsync(user);
        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        var formated = key.FormatKey();

        var uri = _urlEncoder.GenerateQrCodeUri(user.Email, key);

        return new GetTwoFactorResponse()
        {
            ManualKey = formated,
            QrUri = uri
        };

    }
}


