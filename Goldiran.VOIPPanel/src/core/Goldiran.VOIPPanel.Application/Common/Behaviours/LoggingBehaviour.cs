using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Voip.Framework.Domain.CommonServices.IServices;

namespace Goldiran.VOIPPanel.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _contextAccessor;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _contextAccessor=contextAccessor;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? null;
        string userName = string.Empty;
    
        _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request} ",
            requestName, userId, userName, request); 
    }
}
