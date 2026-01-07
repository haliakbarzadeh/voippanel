using Framework.Common.SMS;
using Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.CodeProvider;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Entities.Identity;
using Saramad.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.VerifyLoginWithOTPCommand;

public class VerifyLoginWithOTPCommand :  IRequest<LoginResponse>
{
    public long UserId { get; set; }
    public string Code { get; set; }
}

public class Handler : IRequestHandler<VerifyLoginWithOTPCommand, LoginResponse>
{
    private readonly IApplicationDbContext _db;
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ICodeProvider _codeProvider;
    private ISMSSender _smsSender;

    public Handler(IApplicationDbContext db, IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions, IHttpContextAccessor httpContextAccessor, ICodeProvider codeProvider, ISMSSender smsSender)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
        _siteOptions = siteOptions;
        _iTokenService = iTokenService;
        _httpContextAccessor = httpContextAccessor;
        _codeProvider = codeProvider;
        _smsSender = smsSender;
    }

    public async Task<LoginResponse> Handle(VerifyLoginWithOTPCommand request, CancellationToken cancellationToken)
    {
        var user =await _userManager.FindByUserIdAsync(request.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری و رمز عبور معتبر نمی باشد.");
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

        var result = _codeProvider.VerifyVerificationCode(user.UserName, request.Code);
      
        if (!string.IsNullOrEmpty(result))
        {
            var cachSplit=result.Split(';');
            Tuple<string, string>? token;
            if(cachSplit.Count()>1)
                token = await _iTokenService.GenerateAccessToken(user,Convert.ToInt64(cachSplit[1]));
            else
                token = await _iTokenService.GenerateAccessToken(user);

            await _db.LoginHistorys.AddAsync(new LoginHistory()
            {
                UserId = user.Id,
                ActionDate = DateTime.Now,
                Success = true,
                LoginFunctionType = LoginFunctionTypeEnum.SuccessLogin,
                Ip = _httpContextAccessor.HttpContext?.Items["UserIp"].ToString()

            });

            await _db.SaveChangesAsync(cancellationToken);
            return new LoginResponse()
            {
                IsTemporary = false,
                //UserId= user.Id,
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }
        else 
        {
            throw new InvalidOperationException("عدم دسترسی ورود.");
         
        }

       
    }
}


