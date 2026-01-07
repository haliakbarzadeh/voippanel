using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetUserPositionsListQuery
{
    public long? UserId { get; set; }
    public long? PositionId { get; set; }
    public bool? IsActive { get; set; }
}