using Framework.Attributes.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Saramad.Core.ApplicationService.Common.Models;
using Saramad.Core.ApplicationService.Features.ApplicationRoleAssignments.Commands.CreateApplicationRoleAssignment;
using Saramad.Core.ApplicationService.Features.ApplicationRoleAssignments.Commands.DeleteApplicationRoleAssignment;
using Saramad.Core.ApplicationService.Features.ApplicationRoleAssignments.Dtos;
using Saramad.Core.ApplicationService.Features.ApplicationRoleAssignments.Queries.GetApplicationRoleAssignment;
using Saramad.Core.ApplicationService.Features.ApplicationRoleAssignments.Queries.GetApplicationRoleAssignments;

namespace Saramad.EndPoints.Api.Controllers;
[Authorize(Policy = "Roles")]
public class ApplicationRoleAssignmentsController : AuthorizeApiControllerBase
{
    [Authorize(Policy = "Roles_create")]
    [HttpPost]
    //[HasPermission("ApplicationRoleAssignment:create")]
    public async Task<ActionResult<long>> Create(CreateApplicationRoleAssignmentCommand command)
    {
        return await Mediator.Send(command);
    }

    [Authorize(Policy = "Roles_get")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("ApplicationRoleAssignment:read")]
    public async Task<ActionResult<ApplicationRoleAssignmentDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetApplicationRoleAssignmentQuery() { Id = id }));
    }

    [Authorize(Policy = "Roles_get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("ApplicationRoleAssignment:search")]
    public async Task<ActionResult<PaginatedList<ApplicationRoleAssignmentDto>>> List([FromQuery] GetApplicationRoleAssignmentsQuery query)
    {
        return await Mediator.Send(query);
    }

    [Authorize(Policy = "Roles_create")]
    [HttpDelete("{id}")]
    //[HasPermission("ApplicationRoleAssignment:delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteApplicationRoleAssignmentCommand() { Id=id});
        return NoContent();
    }

}