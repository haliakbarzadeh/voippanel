using Framework.Common.Security;
using Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.Identity;
using Saramad.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.LogoutCommand;

public class LogoutCommand :  IRequest<bool>
{
    [JsonIgnore]
    public string AccessToken { get; set; }
}

public class Handler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Handler(IApplicationDbContext db, IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var principal = _iTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal.Identity.GetUserId();

        var user = await _userManager.FindByIdAsync(username);
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("اکانت شما غیرفعال شده‌است.");
        }

        var result = await _db.Tokens.Where(c => c.UserId == Convert.ToInt64(username) && c.TokenValue==request.AccessToken).FirstOrDefaultAsync();
        if (result != null)
        {
            _db.Tokens.Remove(result);
            await _db.SaveChangesAsync(cancellationToken);
        }

        await _db.LoginHistorys.AddAsync(new LoginHistory()
        {
            UserId = user.Id,
            ActionDate = DateTime.Now,
            Success = true,
            LoginFunctionType = LoginFunctionTypeEnum.SuccessLogout,
            Ip = _httpContextAccessor.HttpContext?.Items["UserIp"].ToString()

        });

        await _db.SaveChangesAsync(cancellationToken);

        return true;

    }
}


