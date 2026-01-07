using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class FileRowDto 
{
    public Guid Id { get; set; }
    public FileOwnerType FileOwnerTypeId { get; set; }
    //public string FileOwnerUsage { get; set; }
    //public Guid? FileOwnerId { get; set; }
    //public Guid SessionId { get; set; }

    public string FileName { get; set; }
    public string Name { get; set; }
    public long Length { get; set; }
    public string ContentType { get; set; }

}