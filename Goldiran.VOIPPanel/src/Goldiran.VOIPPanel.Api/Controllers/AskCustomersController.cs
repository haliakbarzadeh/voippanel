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

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "monitoredunits")]
public class AskCustomersController : ApiControllerBase
{

    //[AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<AskCustomerDto>>> List([FromQuery] GetAskCustomersQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getmaster")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<MasterAskCustomerDto>> GetMaster([FromQuery] GetMasterAskCustomersQuery query)
    {
        return await Mediator.Send(query);
    }

}