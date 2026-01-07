using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class PositionMapper : Profile
{
    public PositionMapper()
    {
        CreateMap<Position, PositionDto>(MemberList.Destination)
            .ForMember(c=>c.ParentPositionTitle, c=>c.MapFrom(e=>e.ParentPosition.ContactedParentPositionName));
    }
}
