using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class UserPositionMapper : Profile
{
    public UserPositionMapper()
    {
        CreateMap<UserPosition, UserPositionDto>(MemberList.Destination)
            .ForMember(c => c.User, c => c.MapFrom(e => e.User.PersianFullName))
            .ForMember(c => c.Position, c => c.MapFrom(e => e.Position.ContactedParentPositionName));
        CreateMap<UserPosition, QueuUserDto>(MemberList.Destination)
            .ForMember(c => c.PersianFullName, c => c.MapFrom(d => d.User.PersianFullName))
            .ForMember(c => c.UserName, c => c.MapFrom(d => d.User.UserName));

    }
}
