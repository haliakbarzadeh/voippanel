using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetUsersQuery : BaseQueryRequest, IRequest<PaginatedList<UserDto>>
{
    public string PersianFullName { get; set; }=string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public bool? IsComfired { get; set; }
}