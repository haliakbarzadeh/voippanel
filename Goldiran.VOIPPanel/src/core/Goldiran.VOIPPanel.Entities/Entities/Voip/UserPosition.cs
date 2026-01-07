using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class UserPosition : BaseQueryEntity<long>
{
    public DateTime StartDate { get;  set; }
    public DateTime? EndDate { get;  set; }
    public DateTime? ExpireDate { get;  set; }
    public bool IsActive { get;  set; }
    //public bool IsContentAccess { get;  set; }
    public string Queues { get;  set; }
    public string Extension { get;  set; }
    public string? ServerIp { get;  set; }
    public long UserId { get;  set; }
    public virtual AppUser User { get; set; }
    public long PositionId { get;  set; }
    public virtual Position Position { get; set; }
}