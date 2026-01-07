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

namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "Queu")]
public class FlatDataJobController : ApiControllerBase
{

    [Route("getlastflatdata")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FlatDataJobDto>> GetLastFlatData([FromQuery] GetFlatDataJobLastQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("create")]
    [HttpPost]
    //[HasPermission("OperationSetting:create")]
    public async Task<ActionResult<long>> Create(CreateFlatDataJobCommand command)
    {
        return await Mediator.Send(command);
    }

}