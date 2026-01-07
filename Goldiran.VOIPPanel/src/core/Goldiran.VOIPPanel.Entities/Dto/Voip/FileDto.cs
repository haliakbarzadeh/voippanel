using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class FileDto 
{
    public Guid Id { get; set; }
    public FileOwnerType FileOwnerTypeId { get; set; }
    public Guid? FileOwnerId { get; set; }

    public string FileName { get; set; }
    public string Name { get; set; }
    public long Length { get; set; }
    public string ContentType { get; set; }
    //public bool IsDraft { get; set; }

    public byte[]? Content { get; set; }

    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<Domain.Entities.File, FileDto>()
    //        .ForMember(d => d.Content, opt => opt.MapFrom(s => s.FileContent.Content));
    //}

}