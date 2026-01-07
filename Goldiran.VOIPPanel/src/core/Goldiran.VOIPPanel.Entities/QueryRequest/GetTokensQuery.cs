using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetTokensQuery 
{
    public long? UserId { get; set; }
    public string? RefreshToken { get; set; }
    public string? TokenValue { get; set; }
}