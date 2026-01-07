//using Voip.Framework.Domain;
//using Voip.Framework.Domain.Models;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;

//namespace Goldiran.VOIPPanel.Persistence.Common.Behaviours;

//public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : BaseAudityModel, IRequest<TResponse>
//{
//    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
//    private readonly VOIPPanelContext _uow;
//    public TransactionBehaviour(VOIPPanelContext uow, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
//    {
//        _uow = uow ?? throw new ArgumentException(nameof(IUnitOfWork));
//        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
//    }
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var response = default(TResponse);
//        try
//        {
//            if (_uow.HasActiveTransaction || !request.IsTransactional)
//            {
//                return await next();
//            }

//            var strategy = _uow.GetDatabase().CreateExecutionStrategy();
//            await strategy.ExecuteAsync(async () =>
//            {
//                var transaction = await _uow.BeginTransactionAsync();
//                response = await next();
//                await _uow.CommitTransactionAsync();

//            });

//            return response;
//        }
//        catch (Exception ex)
//        {
//            _uow.RollbackTransaction();
//            var st = new StackTrace(ex, true);
//            // Get the top stack frame
//            var frame = st.GetFrame(0);
//            // Get the line number from the stack frame
//            var line = frame.GetFileLineNumber();
//            _logger.LogError(ex, $"خطا در تراکنش های دیتابیس {frame.GetFileName}:{frame.GetFileLineNumber}");

//            throw;
//        }

//    }

//}

