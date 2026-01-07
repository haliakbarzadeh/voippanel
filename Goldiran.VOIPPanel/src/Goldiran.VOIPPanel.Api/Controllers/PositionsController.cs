using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePosition;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.DeletePosition;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePositionWithUsers;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePositionWithUsers;

namespace Goldiran.VOIPPanel.Api.Controllers;


//[Authorize(Policy = "position")]
public class PositionsController : ApiControllerBase
{
    //[Authorize(Policy = "positioncreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create(CreatePositionCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "positioncreate")]
    [Route("createwithusers")]
    [HttpPost]
    public async Task<ActionResult<long>> CreatePositionWithUsers(CreatePositionWithUsersCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("updatewithusers")]
    [HttpPut]
    public async Task<ActionResult<bool>> UpdatePositionWithUsers(UpdatePositionWithUsersCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "positionlist")]
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("Position:read")]
    public async Task<ActionResult<PositionDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPositionByIdQuery() { Id = id }));
    }

    //[Authorize(Policy = "positionlist")]
    [Route("list")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("Position:search")]
    public async Task<ActionResult<PaginatedList<PositionDto>>> List([FromQuery] GetPositionsQuery query)
    {
        return await Mediator.Send(query);

    }

    [Route("getopertationpositions")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("Position:search")]
    public async Task<ActionResult<PaginatedList<PositionDto>>> GetOperationPositions([FromQuery] GetPositionsQuery query)
    {
        query.IsRestricted = true;
        return await Mediator.Send(query);

    }

    //[Authorize(Policy = "positionupdate")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("Position:update")]
    public async Task<ActionResult> Update(long id,UpdatePositionCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    //[Authorize(Policy = "Position_delete")]
    [Route("positiondelete")]
    [HttpDelete]
    //[HasPermission("Position:delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await Mediator.Send(new DeletePositionCommand() { Id=id});
        return NoContent();
    }

}