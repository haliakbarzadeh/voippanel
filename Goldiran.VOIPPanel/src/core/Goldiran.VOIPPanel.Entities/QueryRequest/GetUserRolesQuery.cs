using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;
public class GetUserRolesQuery : BaseQueryRequest, IRequest<PaginatedList<UserRoleDto>>
{
    public long? UserId { get; set; }
    public long? RoleId { get; set; }
}