using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.ChangeGroupUserStatus;


namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class OperationsController : ApiControllerBase
{
    //[Authorize(Policy = "monitoredunitscreate")]
    [Route("changeuserstatus")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<long>> ChangeUserStatus(ChangeUserStatusCommand command)
    {
        return await Mediator.Send(command);
    }
    [Route("changeuserstatusbysupervisor")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<long>> ChangeUserStatus(ChangeUserStatusBySupervisorCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("changegroupuserstatus")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<bool>> ChangeGroupUserStatus(ChangeGroupUserStatusCommand command)
    {
        return await Mediator.Send(command);
    }



    ////[Authorize(Policy = "MonitoredUnits_get")]
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("MonitoredDepartment:read")]
    public async Task<ActionResult<OperationDto>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetOperationByIdQuery() { Id = id }));
    }

    ////[Authorize(Policy = "MonitoredUnits_get")]
    [Route("getlist")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<OperationDto>>> List([FromQuery] GetOperationsQuery query)
    {
        return await Mediator.Send(query);
    }


    ////[Authorize(Policy = "MonitoredUnits_get")]
    //GetGroupMonitoredDepartmentsQuery
    [Route("getcurrentgrouplist")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("MonitoredDepartment:read")]
    public async Task<ActionResult<PaginatedList<AggregateOperationDto>>> GetCurrentGroupList([FromQuery] GetGroupOperationsQuery query)
    {
        query.IsCurrentUser = true;
        return Ok(await Mediator.Send(query));
    }

    [Route("getcurrentoperationlist")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("MonitoredDepartment:read")]
    public async Task<ActionResult<PaginatedList<AggregateOperationDto>>> GetCurrentOperationList([FromQuery] GetGroupOperationsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [Route("getcurrent")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<OperationDto>> GetCurrent([FromQuery] GetOperationQuery query)
    {
        query.IsCurrentUser = true;
        return await Mediator.Send(query);
    }



}