using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Entities;

//using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using System.Text.Json.Serialization;
using Voip.Framework.Common.Mappings;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

//public class UserRoleDto : IMapFrom<AppUserRole>
public class UserRoleDto 

{
    [JsonIgnore]
    public AppUser User { get; set; }
    public string? UserFullName { get { return User.PersianFullName; } }
    public long UserId { get; set; }
    [JsonIgnore]
    public AppRole Role { get; set; }
    public string? RoleName { get { return Role.Name; } }
    public long RoleId { get; set; }
    //public bool IsDeleted { get; set; }

    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<AppUserRole, UserRoleDto>()
    //        .ForMember(d => d.UserFullName, opt => opt.Ignore())
    //        .ForMember(d => d.RoleName, opt => opt.Ignore());
    //}
}