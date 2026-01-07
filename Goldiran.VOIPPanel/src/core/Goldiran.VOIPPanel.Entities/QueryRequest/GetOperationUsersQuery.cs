using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetOperationUsersQuery : BaseQueryRequest, IRequest<PaginatedList<UserPositionDto>>
{
    [BindNever]
    public bool IsRestricted { get; set; } = false;
    public bool? IsActivePosition { get; set; } = true;
    public List<long> PositionIds { get; set; } = new List<long>();

}