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
public class TempFlatDataJobController : ApiControllerBase
{

    [Route("getlastFlatdata")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TempFlatDataJobDto>> GetLastTempFlatData([FromQuery] GetTempFlatDataJobLastQuery query)
    {
        return await Mediator.Send(query);
    }

    [Route("create")]
    [HttpPost]
    //[HasPermission("OperationSetting:create")]
    public async Task<ActionResult<long>> Create(CreateTempFlatDataJobCommand command)
    {
        return await Mediator.Send(command);
    }

}