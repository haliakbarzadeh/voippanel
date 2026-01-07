using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateUserPosition;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.UpdateUserPosition;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Api.Controllers;

[Authorize]
public class UserPositionsController : ApiControllerBase
{
    //[Authorize(Policy = "positioncreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("UserPositions:create")]
    public async Task<ActionResult<long>> Create(CreateUserPositionCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("UserPositions:read")]
    public async Task<ActionResult<UserPositionDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetUserPositionByIdQuery() { Id = id }));
    }

    [Route("list")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("UserPositions:search")]
    public async Task<ActionResult<PaginatedList<UserPositionDto>>> List([FromQuery] GetUserPositionsQuery query)
    {
        return await Mediator.Send(query);
    }

    [AllowAnonymous]
    [Route("getoperationusers")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("UserPositions:search")]
    public async Task<ActionResult<PaginatedList<UserPositionDto>>> GetOperationUsers([FromQuery] GetUserPositionsQuery query)
    {
        var customQuery = new GetOperationUsersQuery()
        {
            IsRestricted = true,
            IsActivePosition = null,
            PositionIds=query.PositionIds,
            Search=query.Search,
            PageNumber=query.PageNumber,
            PageSize=query.PageSize,
            DisablePaging = query.DisablePaging,
        };

        return await Mediator.Send(customQuery);
    }

    //[Authorize(Policy = "positioncreate")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("UserPositions:update")]
    public async Task<ActionResult> Update(long id,UpdateUserPositionCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    //[Authorize(Policy = "positioncreate")]
    [Route("delete")]
    [HttpDelete]
    //[HasPermission("UserPositions:delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteUserPositionCommand() { Id=id});
        return NoContent();
    }

}