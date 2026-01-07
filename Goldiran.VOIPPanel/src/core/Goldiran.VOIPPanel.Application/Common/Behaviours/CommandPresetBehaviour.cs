using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MediatR;
using Voip.Framework.Domain.CommonServices.IServices;
using Voip.Framework.Domain.Models;
using Goldiran.VOIPPanel.Application.Common.Constants;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Common.Behaviours;

public class CommandPresetBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseAudityModel
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IReadModelContext _readModelContext;

    public CommandPresetBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IReadModelContext readModelContext, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _contextAccessor= contextAccessor;
        _readModelContext = readModelContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        request.UserId= (_currentUserService.UserId != null) ? Convert.ToInt64(_currentUserService.UserId) : null; 
        request.PositionId = (_currentUserService.PosId != null) ? Convert.ToInt64(_currentUserService.PosId) : null;
        request.UserIp= (_currentUserService.UserIp != null) ? _currentUserService.UserIp : string.Empty ;
        //
        if (request.PositionId != null && QueryPermissions.HasVerticalPrivilege.FirstOrDefault(c => c.Key == typeof(TRequest)).Value != null && QueryPermissions.HasVerticalPrivilege.FirstOrDefault(c => c.Key == typeof(TRequest)).Value == true)
        {
            request.HasVerticalSecurity = true;

            var position = _readModelContext.Set<Position>().AsNoTracking().Where(c => c.Id == request.PositionId).FirstOrDefault();

            if (position.IsContentAccess)
            {
                request.HasContentAccess = true;
            }
            else
            {
                request.HasContentAccess = false;
                var childPositions=_readModelContext.Set<Position>().AsNoTracking().Where(c=>EF.Functions.Like(c.ContactedParentPositionId, $"%'{request.PositionId}'%")).Select(c=>c.Id).ToList();
                if (childPositions.Any())
                    request.PositionCildIds.AddRange(childPositions);

                var monitoresPositions= _readModelContext.Set<MonitoredPosition>().AsNoTracking().Where(c =>c.SourcePositionId==request.PositionId).Select(c => c.DestPositionId).ToList();
                if (monitoresPositions.Any())
                    request.MonitoredPositionIds.AddRange(monitoresPositions);
            }
        }
        else
        {
            request.HasContentAccess=true;
        }

        return await next();

    }
}
