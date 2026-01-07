using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Goldiran.VOIPPanel.Api.Models;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.Queus.Commands.AddUserToQueue;
using Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.CreateOperationSetting;
using Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.UpdateOperationSetting;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;
using Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu;
using Goldiran.VOIPPanel.Application.Features.Queus.Commands.UpdateQueu;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "Queu")]
public class QueusController : ApiControllerBase
{

    [Route("addqueuetouser")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AddQueueToUsers(AddUserToQueueCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("getqueues")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QueuDto>>> GetQueues([FromQuery] GetQueusQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getpositionqueues")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QueuDto>>> GetPositionQueues([FromQuery] GetPositionQueusQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getqueueusers")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QueuUserDto>>> GetQueueUsers([FromQuery] GetQueusUsersQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getseperatedqueueusers")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<QueusUsersResponse>> GetSeperatedQueueUsers([FromQuery] GetSeperatedQueusUsersQuery query)
    {
        return await Mediator.Send(query);
    }


    [Route("create")]
    [HttpPost]
    //[HasPermission("OperationSetting:create")]
    public async Task<ActionResult<int>> Create(CreateQueuCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "OperationSettingupdate")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("OperationSetting:update")]
    public async Task<ActionResult> Update(int id, UpdateQueuCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    //[Authorize(Policy = "OperationSetting_delete")]
    [Route("delete")]
    [HttpDelete]
    //[HasPermission("OperationSetting:delete")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteQueuCommand() { Id = id });
        return NoContent();
    }
}