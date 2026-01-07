using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetOperationQuery : BaseQueryRequest, IRequest<OperationDto>
{
    public List<long> UserIds { get; set; } = new List<long>();
    public long? User { get; set; }
    public long? Position { get; set; }
    public OperationType? OperationTypeId { get; set; }
    public bool? IsCurrentStatus { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool IsCurrentUser {  get; set; }
}