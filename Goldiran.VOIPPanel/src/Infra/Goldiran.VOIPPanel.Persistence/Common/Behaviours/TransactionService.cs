using Azure;
using Azure.Core;
using Voip.Framework.Domain.Models;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Persistence.Common.Behaviours;

public class TransactionService<TResponse>: ITransactionService<TResponse>
{
    private readonly ILogger<TransactionService< TResponse>> _logger;
    private readonly VOIPPanelContext _uow;
    public TransactionService(VOIPPanelContext uow, ILogger<TransactionService<TResponse>> logger)
    {
        _uow = uow;
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }

    public async Task<TResponse> SetTransaction(BaseAudityModel request, Func<Task<TResponse>> action)
    {
        var response = default(TResponse);
        try
        {
            if (_uow.HasActiveTransaction || !request.IsTransactional)
            {
                return await action();
            }

            var strategy = _uow.GetDatabase().CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transaction = await _uow.BeginTransactionAsync();
                response = await action();
                await _uow.CommitTransactionAsync();

            });

            return response;
        }
        catch (Exception ex)
        {
            _uow.RollbackTransaction();
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            _logger.LogError(ex, $"خطا در تراکنش های دیتابیس {frame.GetFileName}:{frame.GetFileLineNumber}");

            throw;
        }
    }

}
