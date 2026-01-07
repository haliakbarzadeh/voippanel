using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.CreateOperationSetting;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.UpdateOperationSetting;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

namespace Goldiran.VOIPPanel.Api.Controllers;


//[Authorize(Policy = "OperationSetting")]
public class OperationSettingsController : ApiControllerBase
{
    //[Authorize(Policy = "OperationSettingcreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("OperationSetting:create")]
    public async Task<ActionResult<int>> Create(CreateOperationSettingCommand command)
    {
        return await Mediator.Send(command);
    }

    //[Authorize(Policy = "OperationSettinglist")]
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HasPermission("OperationSetting:read")]
    public async Task<ActionResult<OperationSettingDto>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetOperationSettingByIdQuery() { Id = id }));
    }

    //[Authorize(Policy = "OperationSettinglist")]
    [Route("list")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[HasPermission("OperationSetting:search")]
    public async Task<ActionResult<PaginatedList<OperationSettingDto>>> List([FromQuery] GetOperationSettingsQuery query)
    {
        return await Mediator.Send(query);

    }

    //[Authorize(Policy = "OperationSettingupdate")]
    [Route("update")]
    [HttpPut]
    //[HasPermission("OperationSetting:update")]
    public async Task<ActionResult> Update(int id,UpdateOperationSettingCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    //[Authorize(Policy = "OperationSetting_delete")]
    [Route("delete")]
    [HttpDelete]
    //[HasPermission("OperationSetting:delete")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteOperationSettingCommand() { Id=id});
        return NoContent();
    }

}