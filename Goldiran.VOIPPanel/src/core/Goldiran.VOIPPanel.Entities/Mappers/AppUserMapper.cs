using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using RUser=Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class AppUserMapper:Profile
{
    public AppUserMapper()
    {
        CreateMap<RUser.AppUser, UserDto>(MemberList.Destination);
        CreateMap<RUser.AppUser, QueuUserDto>(MemberList.Destination)
            .ForMember(c=>c.Extension,c=>c.MapFrom(d=>!d.UserPositions.IsNullOrEmpty()? d.UserPositions.ToList()[0].Extension : string.Empty));
        CreateMap<AppUser, UserDto>(MemberList.Destination);
        //CreateMap<AppUser, RUser.AppUser>(MemberList.Destination);
        //CreateMap<RUser.AppUser, AppUser>(MemberList.Destination);

    }
}
