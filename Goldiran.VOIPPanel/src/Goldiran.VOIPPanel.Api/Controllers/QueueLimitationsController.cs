using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.CreateQueueLimitation;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateQueueLimitation;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateHourValue;
using Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.DeleteHourValue;

namespace Goldiran.VOIPPanel.Api.Controllers;


//[Authorize(Policy = "QueueLimitation")]
public class QueueLimitationsController : ApiControllerBase
{
    //[Authorize(Policy = "QueueLimitationcreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("QueueLimitation:create")]
    public async Task<ActionResult<int>> Create(CreateQueueLimitationCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "QueueLimitationlist")]
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("QueueLimitation:read")]
    public async Task<ActionResult<QueueLimitationDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetQueueLimitationByIdQuery() { Id = id }));
    }

    //[Authorize(Policy = "QueueLimitationlist")]
    [Route("list")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("QueueLimitation:search")]
    public async Task<ActionResult<PaginatedList<QueueLimitationDto>>> List([FromQuery] GetQueueLimitationsQuery query)
    {
        return await Mediator.Send(query);

    }

    //[Authorize(Policy = "QueueLimitationupdate")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("QueueLimitation:update")]
    public async Task<ActionResult> Update(int id,UpdateQueueLimitationCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [Route("updatehourevalue")]
    [HttpPut]
    //[HasPermission("QueueLimitation:update")]
    public async Task<ActionResult> UpdateHoureValue(UpdateHourValueCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [Route("deletehourevalue")]
    [HttpDelete]
    //[HasPermission("QueueLimitation:update")]
    public async Task<ActionResult> DeleteHoureValue(DeleteHourValueCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    //[Authorize(Policy = "QueueLimitation_delete")]
    [Route("QueueLimitationdelete")]
    [HttpDelete]
    //[HasPermission("QueueLimitation:delete")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteQueueLimitationCommand() { Id=id});
        return NoContent();
    }

}