using Framework.Common.Security;
using Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.Services;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RevokeCommand;

public class RevokeCommand : BaseCreateCommandRequest, IRequest<bool>
{
    [JsonIgnore]
    public string AccessToken { get; set; }
}

public class Handler : IRequestHandler<RevokeCommand, bool>
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

    public async Task<bool> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var principal = _iTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.Identity.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("اکانت شما غیرفعال شده‌است.");
        }


        var result = await _db.Tokens.Where(c => c.UserId ==request.UserId && c.TokenValue==request.AccessToken).FirstOrDefaultAsync();
        if (result != null)
        {
           _db.Tokens.Remove(result);
            await _db.SaveChangesAsync(cancellationToken);
        }


        return true;
    }
}

