using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class MonitoredPositionMapper : Profile
{
    public MonitoredPositionMapper()
    {
        CreateMap<MonitoredPosition, MonitoredPositionDto>(MemberList.Destination)
            .ForMember(c => c.DestPositionTitle, c => c.MapFrom(e => e.DestPosition.ContactedParentPositionName))
            .ForMember(c => c.SourcePositionTitle, c => c.MapFrom(e => e.SourcePosition.ContactedParentPositionName));

        CreateMap<MonitoredPosition, GroupMonitoredPositionDto>(MemberList.Destination)
            .ForMember(c => c.SourcePositionTitle, c => c.MapFrom(e => e.SourcePosition.ContactedParentPositionName));
    }
}
