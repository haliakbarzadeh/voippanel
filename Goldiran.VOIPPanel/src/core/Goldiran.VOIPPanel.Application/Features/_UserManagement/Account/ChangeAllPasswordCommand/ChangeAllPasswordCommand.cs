using Framework.Common.Collections;
using Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Saramad.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using ValidationException = Saramad.Core.ApplicationService.Common.Exceptions.ValidationException;

namespace Saramad.Core.ApplicationService.Features.Account.ChangeAllPasswordCommand;

public class ChangeAllPasswordCommand : BaseCreateCommandRequest, IRequest<bool>
{

}

public class Handler : IRequestHandler<ChangeAllPasswordCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private IApplicationUserManager _userManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
    private ITokenService _iTokenService;

    public Handler(IApplicationDbContext db, IApplicationUserManager userManager, IApplicationSignInManager signInManager, ITokenService iTokenService, IOptionsSnapshot<SiteSettings> siteOptions)
    {
        _db = db;
        _userManager= userManager;
        _signInManager = signInManager;
        _siteOptions= siteOptions;
        _iTokenService= iTokenService;
    }

    public async Task<bool> Handle(ChangeAllPasswordCommand request, CancellationToken cancellationToken)
    {
        var userList = await _userManager.GetAllUsersAsync();
        //var user = _userManager.FindById(797);
        //var tresult1 = await _userManager.ChangePasswordAsync(user, "12345678", "12345");
        //var tresult = await _userManager.ChangePasswordAsync(user, "!23hH438", user.Mobile.Substring(user.Mobile.Length - 6, 5));
        if (userList.IsNullOrEmpty())
        {
            return true;
        }

        IdentityResult result = null;
        foreach (var item in userList)
        {
            if(!string.IsNullOrEmpty(item.Mobile) && item.Mobile.Length>5)
                try
                {
                    //var token = await _userManager.GeneratePasswordResetTokenAsync(item);
                    //result = await _userManager.ResetPasswordAsync(item, token, item.Mobile.Substring(item.Mobile.Length - 6, 5));
                    //await _userManager.UpdateAsync(item);
                    result = await _userManager.ChangePasswordAsync(item, "12345", item.Mobile.Substring(item.Mobile.Length - 6, 5));
                    //item.IsTemperory = true;

                    //await _userManager.UpdateAsync(item);
                }
                catch (Exception ex)
                {

                    try
                    {
                        result = await _userManager.ChangePasswordAsync(item, "1qaz!QAZ", item.Mobile.Substring(item.Mobile.Length - 6, 5));
                        //item.IsTemperory = true;

                        //await _userManager.UpdateAsync(item);
                    }
                    catch (Exception ex1)
                    {


                    }
                }



        }
            

        return true;
        
    }
}


