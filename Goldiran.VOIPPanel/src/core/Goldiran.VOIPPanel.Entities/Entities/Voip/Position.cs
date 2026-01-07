using Microsoft.AspNetCore.Identity;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class Position : BaseQueryEntity<long>
{
    public string Title { get;  set; }
    public PositionType PositionTypeId { get;  set; }
    public long? ParentPositionId { get;  set; }
    public Position? ParentPosition { get;  set; }
    public string? ContactedParentPositionId { get;  set; }
    public string? ContactedParentPositionName { get;  set; }
    public bool IsContentAccess { get;  set; }
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }
    //public virtual ICollection<AppUser> Users { get; set; }
    public virtual ICollection<UserPosition> UserPositions { get; set; }

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Title)? string.Empty:Title;
    }
}