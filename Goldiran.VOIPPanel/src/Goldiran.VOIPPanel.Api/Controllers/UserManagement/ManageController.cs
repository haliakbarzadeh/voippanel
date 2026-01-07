using Goldiran.VOIPPanel.Api.Controllers;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Account.ChangePasswordCommand;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Goldiran.VOIPPanel.Api.Controllers.UserManagement;

[Authorize]
public class ManageController : ApiControllerBase
{

    [Route("changepassword")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<bool>> ChangePassword(ChangePasswordCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("resetpassword")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
    {
        return await Mediator.Send(command);
    }

    //[AllowAnonymous]
    //[Route("changefirstpassword")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> ChangeFirstPassword(ChangePasswordCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    //[Route("setpassword")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> SetPassword(SetUserPasswordCommand command)
    //{
    //    return await Mediator.Send(command);
    //}


    //[Route("enabletwofactor")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> EnableTwoFactor(EnableTwoFactorCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

}