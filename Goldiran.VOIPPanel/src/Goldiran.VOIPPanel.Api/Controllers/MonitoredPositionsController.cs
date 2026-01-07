using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Commands.CreateMonitoredPosition;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class MonitoredPositionsController : ApiControllerBase
{
    //[Authorize(Policy = "monitoredunitscreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<bool>> Create(CreateMonitoredPositionCommand command)
    {
        return await Mediator.Send(command);
    }

    
    ////[Authorize(Policy = "MonitoredUnits_get")]
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("MonitoredDepartment:read")]
    public async Task<ActionResult<MonitoredPositionDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMonitoredPositionByIdQuery() { Id = id }));
    }

    ////[Authorize(Policy = "MonitoredUnits_get")]
    [Route("getlist")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<MonitoredPositionDto>>> List([FromQuery] GetMonitoredPositionsQuery query)
    {
        return await Mediator.Send(query);
    }


    ////[Authorize(Policy = "MonitoredUnits_get")]
    //GetGroupMonitoredDepartmentsQuery
    [Route("getgrouplist")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("MonitoredDepartment:read")]
    public async Task<ActionResult<PaginatedList<GroupMonitoredPositionDto>>> GetGroupList([FromQuery] GetGroupMonitoredPositionsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    //[Authorize(Policy = "monitoredunitscreate")]
    [Route("delete")]
    [HttpDelete]
    //[HasPermission("MonitoredDepartment:delete")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMonitoredPositionCommand() { SourcePositionId = id});
        return NoContent();
    }


}