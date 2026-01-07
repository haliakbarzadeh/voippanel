using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File= Goldiran.VOIPPanel.ReadModel.Entities.File;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class FileMapper : Profile
{
    public FileMapper()
    {
        CreateMap<File, FileDto>(MemberList.Destination)
        .ForMember(c => c.Content, e => e.MapFrom(c => c.FileContent.Content));
        CreateMap<File, FileRowDto>(MemberList.Destination);
        CreateMap<Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File, FileDisplayDto>(MemberList.Destination);

    }
}
