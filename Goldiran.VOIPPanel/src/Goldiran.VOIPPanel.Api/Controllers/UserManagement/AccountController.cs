using Goldiran.VOIPPanel.Application.Features.UserManagement.Account.LoginCommand;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RefreshCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Voip.Framework.Domain.CommonServices.IServices;


namespace Goldiran.VOIPPanel.Api.Controllers.UserManagement;

//////[Authorize(Policy ="test")]
//[Authorize]
//[AllowAnonymous]
public class AccountController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public AccountController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    //[AllowAnonymous]
    //[Route("changeallpassword")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> ChangeAllPassword(ChangeAllPasswordCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    //[AllowAnonymous]
    //[Route("forgotpassword")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordCommand command)
    //{

    //    return await Mediator.Send(command);
    //}
    //[AllowAnonymous]
    //[Route("resetpassword")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
    //{
    //    return await Mediator.Send(command);
    //}
    //[Authorize]
    //[Route("gettwofactor")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<GetTwoFactorResponse>> GetTwoFactor(GetTwoFactorCommand command)
    //{
    //    return await Mediator.Send(command);
    //}
    //[NonAction]
    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        return await Mediator.Send(command);
    }

    //[AllowAnonymous]
    //[Route("loginwithotp")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<LoginWithOTPResponse>> LoginWithOTP([FromBody] LoginWithOTPCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    //[AllowAnonymous]
    //[Route("verifywithotp")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<LoginResponse>> VerifyWithOTP([FromBody] VerifyLoginWithOTPCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    [AllowAnonymous]
    [Route("refreshtoken")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshCommand command)
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        command.AccessToken = accessToken;
        return await Mediator.Send(command);
    }
    //[Authorize]
    //[Route("twofactorlogin")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<LoginResponse>> TwoFactorLogin(TwoFactorLoginCommand command)
    //{
    //    return await Mediator.Send(command);
    //}
    //[Authorize]
    //[Route("removetwofactor")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> RemoveTwoFactor(RemoveTwoFactorCommand command)
    //{
    //    return await Mediator.Send(command);
    //}
    //[Authorize]
    //[Route("revoke")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> Revoke(RevokeCommand command)
    //{
    //    var accessToken = await HttpContext.GetTokenAsync("access_token");
    //    command.AccessToken = accessToken;
    //    //var accessToken = await HttpContext.GetTokenAsync("access_token");
    //    //command.AccessToken = accessToken;
    //    return await Mediator.Send(command);
    //}
    //[Authorize]
    //[Route("logout")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> LogOut(LogoutCommand command)
    //{
    //    var accessToken = await HttpContext.GetTokenAsync("access_token");
    //    command.AccessToken = accessToken;
    //    return await Mediator.Send(command);
    //}
    [HttpGet]
    [Route("ip")]
    public ActionResult<string> Ip()
    {
        var result = Ok(_currentUserService.UserIp);

        return result;
    }
}