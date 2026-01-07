using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Common.Services.IdentityService;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Common.AppSettings;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RefreshCommand;

public class RefreshCommand : BaseCreateCommandRequest, IRequest<LoginResponse>
{
    [JsonIgnore]
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; }
}

public class Handler : IRequestHandler<RefreshCommand, LoginResponse>
{
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;
    private readonly IReadModelContext _context;
    private ITokenRepository _tokenRepository;

    public Handler(IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IReadModelContext context, IOptionsSnapshot<SiteSettings> siteOptions, ITokenRepository tokenRepository)
    {
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
        _tokenRepository= tokenRepository;
        _context= context;
    }

    public async Task<LoginResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {

        var principal = _iTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.Identity.GetUserId();
        var posId =Convert.ToInt64(principal.Identity.GetUserClaimValue("positionId"));
        var user = await _userManager.FindByUserIdAsync(Convert.ToInt64(userId));
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری معتبر نمی باشد.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("اکانت شما غیرفعال شده‌است.");
        }


        var result = await _tokenRepository.FindByAsync((c => c.UserId == Convert.ToInt64(userId) && c.TokenValue == request.AccessToken && c.RefreshToken == request.RefreshToken && DateTime.Now < c.RefreshTokenExpiryTime));
        if (result != null)
        {
            var token = await _iTokenService.GenerateAccessToken(user,posId);
            _tokenRepository.Delete(result);
            await _tokenRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return new LoginResponse()
            {
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }
        else
        {
            throw new ForbiddenAccessException();
        }



        throw new InvalidOperationException("عدم دسترسی ورود.");
    }
}

