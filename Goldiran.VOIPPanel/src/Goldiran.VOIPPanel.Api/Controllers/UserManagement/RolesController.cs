using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.ChangeAccesses;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.CreateRole;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.DeleteRole;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.UpdateRole;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Queries;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ChangeUserRole;
using Goldiran.VOIPPanel.Domain.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;


namespace Goldiran.VOIPPanel.Api.Controllers.UserManagement;

//////[Authorize(Policy ="test")]
//[AllowAnonymous]
//[Authorize(Policy = "Roles")]
public class RolesController : ApiControllerBase
{
    //[Authorize(Policy = "Roles_create")]
    ////[Authorize(Policy = "Roles_create")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("Role:create")]
    public async Task<ActionResult<bool>> Create(CreateRoleCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "Roles_get")]
    [Route("getrole")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //////[Authorize(Policy = "test")]
    public async Task<ActionResult<RoleDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetRoleByIdQuery() { Id = id }));
    }

    //[Authorize(Policy = "Roles_get")]
    [Route("getroles")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("ApplicationRole:search")]
    public async Task<ActionResult<PaginatedList<RoleDto>>> List([FromQuery] GetRolesQuery query)
    {
        return await Mediator.Send(query);
    }


    //[Authorize(Policy = "Roles_update")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("ApplicationRole:update")]
    public async Task<ActionResult> Update(long id, UpdateRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    ////[Authorize(Policy = "Roles_create")]
    [Route("changeuserrole")]
    [HttpPut]
    //[HasPermission("ApplicationRole:update")]
    public async Task<ActionResult> ChangeUserRole(long id, ChangeUserRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }


    //[Authorize(Policy = "AccessManagement_create")]
    [Route("changeaccess")]
    [HttpPut]
    //[HasPermission("ApplicationRole:update")]
    public async Task<ActionResult> ChangeAccess(long id, ChangeAccessesCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    //[Authorize(Policy = "Roles_delete")]
    [Route("deleterole")]
    [HttpDelete]
    //[HasPermission("psp:delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteRoleCommand(id));

        return NoContent();
    }

    ////[Authorize(Policy = "AccessManagement_get")]
    //[Authorize(Policy = "AccessManagement_create")]
    [Route("getrolepermission")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //////[Authorize(Policy = "test")]
    public async Task<ActionResult<List<PermissionNodes>>> GetRolePermission(long roleId)
    {
        return Ok(await Mediator.Send(new GetRolePermission() { Id = roleId }));
    }
}