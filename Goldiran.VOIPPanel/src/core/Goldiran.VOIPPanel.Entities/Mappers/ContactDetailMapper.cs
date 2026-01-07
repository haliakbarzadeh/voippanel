using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class ContactDetailMapper : Profile
{
    public ContactDetailMapper()
    {
        CreateMap<ContactDetail, ContactDetailDto>(MemberList.Destination)
            .ForMember(c=>c.RealDisposition,c=>c.MapFrom(d=>d.Disposition));
        CreateMap<ContactDetail, AutoDialDto>(MemberList.Destination)
                    .ForMember(c => c.RealDisposition, c => c.MapFrom(d => d.Disposition));
    }
}
