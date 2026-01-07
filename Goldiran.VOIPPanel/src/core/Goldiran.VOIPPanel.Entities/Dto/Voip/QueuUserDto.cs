using AutoMapper;
using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;


namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class QueuUserDto
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string PersianFullName { get; set; }
    public string Extension {  get; set; }
    public string ExtensionName { get { return $"{Extension}_{PersianFullName}"; } }

}