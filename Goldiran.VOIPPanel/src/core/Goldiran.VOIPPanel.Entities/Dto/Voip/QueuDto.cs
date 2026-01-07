using AutoMapper;
using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;


namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class QueuDto
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public string IPAddress { get; set; }
    public string User { get; set; }
    public string Secret { get; set; }
    public string FullName { get { return $"{Code}_{Name}"; } }
    public int Count { get; set; }
    public bool IsSLA { get; set; }
    public bool IsFCR { get; set; }

}