using Voip.Framework.Caching.Caching;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Common.Services.CodeProvider;
using Goldiran.VOIPPanel.Application.Common.Services.IdentityService;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using System.ComponentModel.DataAnnotations;
using System.Security;
using Voip.Framework.Common.AppSettings;
using Voip.Framework.Common.Extensions;



namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.LoginCommand;

public class LoginCommand :  IRequest<LoginResponse>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? SecurityCode { get; set; }
    public long? PositionId { get; set; }
    public bool RememberMe { get; set; }
}

public class Handler : IRequestHandler<LoginCommand, LoginResponse>
{
    private IAppUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ICodeProvider _codeProvider;
    private readonly IDistributedCache _cach;
    private readonly IReadModelContext _context;

    public Handler(IAppUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions, IHttpContextAccessor httpContextAccessor, ICodeProvider codeProvider, IReadModelContext context, IDistributedCache cach)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _siteOptions = siteOptions;
        _iTokenService = iTokenService;
        _httpContextAccessor = httpContextAccessor;
        _codeProvider = codeProvider;
        _cach = cach;
        _context = context;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        Tuple<string, string>? token;
        //if (string.IsNullOrEmpty(request.SecurityCode))
        //{
        //    throw new InvalidOperationException("کد امنیتی را وارد کنید");
        //}

        //try
        //{
        //    var cachResult = _cach.Get<string>(request.SecurityCode.ToLower());

        //    if (cachResult == null) { throw new InvalidOperationException("کد امنیتی صحیح نمی باشد"); }
        //}
        //catch (Exception ex)
        //{

        //    throw new InvalidOperationException("کد امنیتی صحیح نمی باشد");
        //}


        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            throw new InvalidOperationException("نام کاربری  و رمز عبور معتبر نمی باشد.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("اکانت شما غیرفعال شده‌است.");
        }

        //if (_siteOptions.Value.EnableEmailConfirmation &&
        //    !await _userManager.IsEmailConfirmedAsync(user))
        //{
        //    throw new InvalidOperationException("لطفا به پست الکترونیک خود مراجعه کرده و ایمیل خود را تائید کنید!");
        //}

        var result = await _signInManager.PasswordSignInAsync(
                                request.UserName,
                                request.Password,
                                request.RememberMe,
                                lockoutOnFailure: true);
        if (result.Succeeded)
        {
            var position = await _context.Set<UserPosition>().Include(c => c.Position).Where(c=>c.IsActive && c.UserId == user.Id).ToListAsync();
            if (position.IsNullOrEmpty())
            {
                throw new InvalidOperationException($"عدم دسترسی ورود به دلیل عدم تکمیل اطلاعات سازمانی");
            }
            else if (position.Count > 1 && request.PositionId == null)
            {
                return new LoginResponse()
                {
                    IsTemporary = false,
                    //UserId= user.Id,
                    IsMultiPosition = true,
                    PositionList = position.Select(c => new LoginPositions() { Id = c.PositionId, Title = c.Position.Title }).ToList(),

                };
            }
            else if ((position.Count > 1 && request.PositionId != null && !position.Where(c => c.PositionId == request.PositionId && c.IsActive).Any()))
            {
                throw new InvalidOperationException("سمت سازمانی وارد شده متعلق به این کاربر نمی باشد.");
            }
            else if ((position.Count > 1 && request.PositionId != null && position.Where(c => c.PositionId == request.PositionId && c.IsActive).Any()))
            {
                token = await _iTokenService.GenerateAccessToken(user, request.PositionId);
            }
            else
            {
                token = await _iTokenService.GenerateAccessToken(user);
            }

            return new LoginResponse()
            {
                IsTemporary = false,
                //UserId = user.Id,
                //test = code
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }


        if (result.IsLockedOut)
        {
            throw new InvalidOperationException($"{request.UserName} قفل شده‌است.");
        }
        throw new InvalidOperationException("نام کاربری  و رمز عبور معتبر نمی باشد");
    }
}


