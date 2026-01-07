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
public class SecureCallsController : ApiControllerBase
{

    [HttpGet]
    [Route("getsecurecalls")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<SecureCallDto>>> GetSecureCallList([FromQuery] GetSecureCallsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    [Route("getgeneralsecurecall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<GeneralSecureCallDto>> GetGeneralSecureCall([FromQuery] GetGeneralSecureCallQuery query)
    {
        return await Mediator.Send(query);
    }

}