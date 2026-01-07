using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.ChangeGroupUserStatus;
using Goldiran.VOIPPanel.Application.Features.Rasads.Commands.ChanSpy;
using Goldiran.VOIPPanel.Application.Features.Rasads.Commands.QueueRasad;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.Application.Features.Queus.Commands.UpdateQueu;
using Goldiran.VOIPPanel.Application.Features.AnsweringMachines.Commands;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class AnsweringMachinesController : ApiControllerBase
{

    //[AllowAnonymous]
    [Route("getansweringmachine")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<AnsweringMachineDto>>> GetAnsweringMachines([FromQuery] GetAnsweringMachinesQuery query)
    {
        return await Mediator.Send(query);
    }


    [Route("getqueueloganswering")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<QueueLogDto>>> GetQueueLogDetail([FromQuery] GetQueueLogsQuery query)
    {
        query.Queues = new List<int>() { 20 };
        return await Mediator.Send(query);
    }

    [Route("update")]
    [HttpPut]
    //[HasPermission("OperationSetting:update")]
    public async Task<ActionResult> Update(int id, UpdateAnsweringMachineCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}