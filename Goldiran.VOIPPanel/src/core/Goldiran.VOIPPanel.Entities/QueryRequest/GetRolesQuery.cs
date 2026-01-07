using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetRolesQuery : BaseQueryRequest, IRequest<PaginatedList<RoleDto>>
{
    public string Name { get; set; } = string.Empty;
}
