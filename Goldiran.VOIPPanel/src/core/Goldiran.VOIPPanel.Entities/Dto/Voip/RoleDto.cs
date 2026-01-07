using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class RoleDto 
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<UserRoleDto> UserRoleDtoList { get; set; } = new List<UserRoleDto>();

}
