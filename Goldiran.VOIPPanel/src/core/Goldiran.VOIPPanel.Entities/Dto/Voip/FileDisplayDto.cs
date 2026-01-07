using AutoMapper;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class FileDisplayDto 
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string Name { get; set; }
    public long Length { get; set; }
    public string ContentType { get; set; }

}