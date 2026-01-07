using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
public class Position : AggregateRoot<long>
{
    public string Title { get; private set; }
    public PositionType PositionTypeId { get; private set; }
    public long? ParentPositionId { get; private set; }
    public Position? ParentPosition { get; private set; }
    public string? ContactedParentPositionId { get; private set; }
    public string? ContactedParentPositionName { get; private set; }
    public bool IsContentAccess {  get; private set; }
    public ShiftType? ShiftType { get; private set; }
    public bool? HasShifts { get; private set; }

    public Position(string title, PositionType positionTypeId, long? parentPositionId, string contactedParentPositionId, string contactedParentPositionName, bool isContentAccess, ShiftType? shiftType, bool? hasShifts)
    {
        Title = title;
        PositionTypeId = positionTypeId;
        ParentPositionId = parentPositionId;
        ContactedParentPositionId = contactedParentPositionId;
        ContactedParentPositionName = contactedParentPositionName;
        IsContentAccess = isContentAccess;
        ShiftType = shiftType;
        HasShifts = hasShifts;
    }

    public void Update(string title, PositionType positionTypeId, long? parentPositionId, string contactedParentPositionId, string contactedParentPositionName, bool isContentAccess, ShiftType? shiftType, bool? hasShifts)
    {
        Title = title;
        PositionTypeId = positionTypeId;
        ParentPositionId = parentPositionId;
        ContactedParentPositionId = contactedParentPositionId;
        ContactedParentPositionName = contactedParentPositionName;
        IsContentAccess=isContentAccess;
        ShiftType = shiftType;
        HasShifts = hasShifts;
    }

    public void UpdateParentPosition(string contactedParentPositionId, string contactedParentPositionName)
    {
        ContactedParentPositionId = contactedParentPositionId;
        ContactedParentPositionName = contactedParentPositionName;
    }
    public void SetContentAccess(bool isContentAccess)
    {
        IsContentAccess = isContentAccess;
    }

}