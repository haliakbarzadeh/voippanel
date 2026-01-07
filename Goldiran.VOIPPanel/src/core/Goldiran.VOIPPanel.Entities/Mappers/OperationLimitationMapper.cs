using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;
public class QueueLimitationMapper : Profile
{
    public QueueLimitationMapper()
    {
        CreateMap<QueueLimitation, QueueLimitationDto>(MemberList.Destination)
            .ForMember(c=>c.HourValues,c=>c.MapFrom(d=>d.HourValues.OrderBy(e=>e.HourTypeId).ToList()));
        CreateMap<HourValue, HourValueDto>(MemberList.Destination);
    }
}
