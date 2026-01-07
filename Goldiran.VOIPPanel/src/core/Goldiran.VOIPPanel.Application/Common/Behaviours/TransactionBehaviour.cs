using Voip.Framework.Domain;
using Voip.Framework.Domain.Models;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Goldiran.VOIPPanel.Application.Common.Behaviours;
public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : BaseAudityModel, IRequest<TResponse>
{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly ITransactionService<TResponse> _transactionServic;
    public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, ITransactionService<TResponse> transactionService)
    {
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        _transactionServic = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await _transactionServic.SetTransaction(request, ()=>{ return next(); });

    }

}

