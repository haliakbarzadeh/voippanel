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
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Commands.CreateFlatContactDetail;
using Goldiran.VOIPPanel.Application.Features.FlatContactDetails.Models;
using Goldiran.VOIPPanel.Application.Features.TempFlatContactDetails.Commands.CreateTempFlatContactDetail;

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "Queu")]
public class FlatContactDetailsController : ApiControllerBase
{

    [Route("createflatcontactdetaillist")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<long>> CreateFlatContactDetailList(CreateFlatContactDetailListCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("createflatcontactdetailjob")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FlatContactDetailResponse>> CreateFlatContactDetailJob(CreateFlatContactDetailListJobCommand command)
    {
        return await Mediator.Send(command);
    }

    [Route("createtempflatcontactdetailjob")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FlatContactDetailResponse>> CreateTempFlatContactDetailJob(CreateTempFlatContactDetailListJobCommand command)
    {
        return await Mediator.Send(command);
    }
    [Route("createflatautodialjob")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FlatContactDetailResponse>> CreateFlatAutoDialJob(CreateFlatAutoDialListJobCommand command)
    {
        return await Mediator.Send(command);
    }

}