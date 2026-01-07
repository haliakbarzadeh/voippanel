using AutoMapper;
using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Voip.Framework.Common.Extensions;


namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class PositionDto 
{
    public long Id { get; set; }
    public PositionType PositionTypeId { get; set; }
    public string PositionType { get { return PositionTypeId!=0? PositionTypeId.Description():string.Empty; } }
    public long? ParentPositionId { get; set; }
    public PositionDto? ParentPosition { get; set; }
    public string? ParentPositionTitle { get; set; }
    //public string? ParentPositionTitle { get { return ParentPositionId != null ? ParentPosition.ContactedParentPositionName : string.Empty; } }
    public string Title { get; set; }
    public string? ContactedParentPositionId { get; set; }
    public string? ContactedParentPositionName { get; set; }
    public bool IsContentAccess { get; set; }
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }
    public string ShiftTypeStr { get { return ShiftType != null ? ShiftType.Description() : string.Empty; } }


}