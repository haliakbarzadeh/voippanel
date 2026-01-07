using Voip.Framework.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.Common.Contracts;

public interface ITransactionService<TResponse>
{
    public Task<TResponse> SetTransaction(BaseAudityModel request, Func<Task<TResponse>> action);

}
