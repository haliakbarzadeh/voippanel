using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class AppRoleMapper : Profile
{
    public AppRoleMapper()
    {
        CreateMap<AppRole, RoleDto>(MemberList.Destination)
        .ForMember(c => c.UserRoleDtoList, e => e.MapFrom(c => c.UserRoles));

        CreateMap<AppUserRole, UserRoleDto>(MemberList.Destination)
            .ForMember(c => c.User, e => e.MapFrom(c => c.User))
            .ForMember(c => c.Role, e => e.MapFrom(c => c.Role));
    }
}
