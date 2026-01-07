using Goldiran.VOIPPanel.Application.Features.SoftPhoneEvents.Commands.CreateSoftPhoneEvent;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Api.Controllers;

public class SoftPhoneEventController : ApiControllerBase
{

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateSoftPhoneEventCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<SoftPhoneEventDto>>> Get([FromQuery] GetSoftPhoneEventsQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }
}
