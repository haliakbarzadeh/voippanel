using AutoMapper;
using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;
public class OperationMapper : Profile
{
    public OperationMapper()
    {
        CreateMap<Operation, OperationDto>(MemberList.Destination)
            .ForMember(c=>c.UserFullName,c=>c.MapFrom(d=>d.User.UserName))
            .ForMember(c=>c.PersianFullName,c=>c.MapFrom(d=>d.User.PersianFullName))
            .ForMember(c => c.Extension, c => c.MapFrom(d => !d.User.UserPositions.IsNullOrEmpty()?d.User.UserPositions.FirstOrDefault(e=>e.PositionId==d.PositionId).Extension:string.Empty))
            .ForMember(c=>c.ManagerUserFullName,c=>c.MapFrom(d=>d.ManagerUser!=null?d.ManagerUser.PersianFullName:string.Empty));
    }
}
