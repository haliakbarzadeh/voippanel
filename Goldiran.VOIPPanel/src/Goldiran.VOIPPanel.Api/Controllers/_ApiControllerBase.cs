using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Goldiran.VOIPPanel.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    private ILogger _logger = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger>();

    protected string? CurrentUserId => string.IsNullOrEmpty(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ? string.Empty : User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

}
