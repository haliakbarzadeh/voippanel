
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ActivateUser;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ChangeUserRole;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.CreateUser;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.DeleteUser;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.UpdateUser;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Goldiran.VOIPPanel.Api.Controllers.UserManagement;

//[Authorize(Policy = "Users")]
//[AllowAnonymous]
//[Authorize]
public class UsersController : ApiControllerBase
{

    //[Authorize(Policy = "userscreate")]
    [Route("createuser")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<bool>> Create( CreateUserCommand command)
    {
        return await Mediator.Send(command);
    }


    //[Authorize(Policy = "usersacivate")]
    [Route("activateuser")]
    [HttpPost]
    //[HasPermission("user:create")]
    public async Task<ActionResult<bool>> ActivateUser(ActivateUserCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "usersacivate")]
    //[Route("confirmuser")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<bool>> ConfirmUser(ConfirmUserCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    //[Authorize(Policy = "usersregister")]
    //[Route("registeruser")]
    //[HttpPost]
    ////[HasPermission("user:create")]
    //public async Task<ActionResult<string>> Register(RegisterUserCommand command)
    //{
    //    return await Mediator.Send(command);
    //}


    //[Authorize(Policy = "usersget")]
    [Route("getuser")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //////[Authorize(Policy = "test")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery() { Id = id }));
    }

    //[Authorize(Policy = "userslist")]
    [Route("getusers")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("ApplicationUser:search")]
    public async Task<ActionResult<PaginatedList<UserDto>>> List([FromQuery] GetUsersQuery query)
    {
        return await Mediator.Send(query);
    }

    //[Authorize(Policy = "userslist")]
    [Route("getuserroles")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("ApplicationUser:search")]
    public async Task<ActionResult<PaginatedList<UserRoleDto>>> GetUserRoles([FromQuery] GetUserRolesQuery query)
    {
        return await Mediator.Send(query);
    }

    //[Authorize(Policy = "orgusers")]
    //[Route("getsbuusers")]
    //[HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    ////[HasPermission("ApplicationUser:search")]
    //public async Task<ActionResult<PaginatedList<UserDto>>> SBUList([FromQuery] GetSBUUsersQuery query)
    //{
    //    return await Mediator.Send(query);
    //}

    //[Authorize(Policy = "usersupdate")]
    [Route("update")]
    [HttpPut]
    public async Task<ActionResult> Update(long id, UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    //[Authorize(Policy = "orgusersupdate")]
    //[Route("updatebydep")]
    //[HttpPut]
    ////[HasPermission("ApplicationUser:update")]
    //public async Task<ActionResult> UpdateByDep(long id, UpdateUserByDepCommand command)
    //{
    //    if (id != command.Id)
    //    {
    //        return BadRequest();
    //    }

    //    await Mediator.Send(command);

    //    return NoContent();
    //}

    //[Authorize(Policy = "usersregister")]
    [Route("changeuserrole")]
    [HttpPut]
    //[HasPermission("ApplicationUser:update")]
    public async Task<ActionResult> ChangeUserRole(long id, ChangeUserRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    //[Authorize(Policy = "usersdelete")]
    [Route("deleteuser")]
    [HttpDelete]
    //[HasPermission("psp:delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteUserCommand() { Id=id});

        return NoContent();
    }

}