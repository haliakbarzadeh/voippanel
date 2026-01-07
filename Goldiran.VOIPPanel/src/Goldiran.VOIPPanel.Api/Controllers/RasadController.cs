using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.ChangeGroupUserStatus;
using Goldiran.VOIPPanel.Application.Features.Rasads.Commands.ChanSpy;
using Goldiran.VOIPPanel.Application.Features.Rasads.Commands.QueueRasad;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.Application.Features.Rasads.Commands.MarkettingRasad;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class RasadController : ApiControllerBase
{

    ////[Authorize(Policy = "MonitoredUnits_get")]
    [Route("getrasadoperation")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<RasadOperationDto>> GetRasadOperations([FromQuery] GetRasadOperationQuery query)
    {
        return await Mediator.Send(query);
    }

    [AllowAnonymous]
    [Route("getrasadcalltype")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<RasadCallTypeDto>> GetRasadCallType([FromQuery] GetRasadCallTypeQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("chanspy")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<bool>> ChanSpy(ChanSpyCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("queuerasad")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<QueueRasadResponse>> QueueRasad(QueueRasadCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("markettingrasad")]
    [HttpPost]
    //[HasPermission("MonitoredDepartment:create")]
    public async Task<ActionResult<QueueRasadResponse>> MarkettingRasad(MarkettingRasadCommand command)
    {
        return await Mediator.Send(command);
    }

}