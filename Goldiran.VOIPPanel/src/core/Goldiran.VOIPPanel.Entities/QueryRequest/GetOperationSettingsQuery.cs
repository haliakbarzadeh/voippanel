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

public class GetOperationSettingsQuery : BaseQueryRequest, IRequest<PaginatedList<OperationSettingDto>>
{
    public OperationType? OperationTypeId { get; set; }
    public bool? IsActive { get; set; }
    public bool? ShowToUser { get; set; }
}
