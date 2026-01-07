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
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class WallboardReportsController : ApiControllerBase
{

    //[AllowAnonymous]
    [HttpGet]
    [Route("getwallboardreport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<WallboardReportDto>> GetWallboard([FromQuery] GetWallboardsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    [Route("getmasterwallboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<MasterWallboardReportDto>> GetMasterWallboard([FromQuery] GetMasterWallboardQuery query)
    {
        return await Mediator.Send(query);
    }

    //[AllowAnonymous]
    [HttpGet]
    [Route("getservicewallboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<ServiceWallboardReportDto>> GetServiceWallboard([FromQuery] GetServiceWallboardsQuery query)
    {
        return await Mediator.Send(query);
    }
}
