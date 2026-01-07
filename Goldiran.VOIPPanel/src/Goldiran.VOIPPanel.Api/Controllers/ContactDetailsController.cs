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
public class ContactDetailsController : ApiControllerBase
{

    //[AllowAnonymous]
    [Route("getcontactdetail")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<ContactDetailDto>>> GetContactDetails([FromQuery] GetContactDetailsQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getautodials")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<ContactDetailDto>>> GetAutoDials([FromQuery] GetContactDetailsQuery query)
    {
        query.ContactReportType = ContactReportType.AutoDial;
        return await Mediator.Send(query);
    }

    [Route("getautodialsnew")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<AutoDialDto>>> GetAutoDialsNew([FromQuery] GetAutoDetailsQuery query)
    {
        query.ContactReportType = ContactReportType.AutoDial;
        return await Mediator.Send(query);
    }

    [Route("getqueuelogdetail")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<PaginatedList<QueueLogDto>>> GetQueueLogDetail([FromQuery] GetQueueLogsQuery query)
    {
        return await Mediator.Send(query);
    }
    [Route("getqueuehourlogdetail")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("MonitoredDepartment:search")]
    public async Task<ActionResult<QueueLogDetailsDto>> GetQueueHourLogDetail([FromQuery] GetDetailsQueueLogsQuery query)
    {
        return await Mediator.Send(query);
    }


    [Route("getcontactdetails")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ContactDetailDto>>> GetContactDetailsByLinkedId([FromQuery] GetContactDetailsByLinkedIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("getgroupedcontactdetails")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ContactDetailDto>>> GetGroupedContactDetails([FromQuery] GetGroupedContactDetailQuery query)
    {
        return await Mediator.Send(query);
    }
}