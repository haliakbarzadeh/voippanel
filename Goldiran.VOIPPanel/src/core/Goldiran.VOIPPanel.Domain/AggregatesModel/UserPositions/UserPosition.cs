using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;

public class UserPosition :AggregateRoot<long>
{
    public long UserId { get; private set; }
    public long PositionId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public bool IsActive { get; private set; }
    public string Queues {  get; private set; }
    public string Extension {  get; private set; }
    public string? ServerIp { get; private set; }

    public UserPosition(long userId, long positionId, DateTime startDate, DateTime? endDate, DateTime? expireDate, bool isActive, string queues, string extension, string serverIp)
    {
        UserId = userId;
        PositionId = positionId;
        StartDate = startDate;
        EndDate = endDate;
        ExpireDate = expireDate;
        IsActive = isActive;
        Queues = queues;
        Extension = extension;
        ServerIp = serverIp;
    }

    public void Update(string queues, string extension, string serverIp)
    {
        Queues = queues;
        Extension = extension;
        ServerIp = serverIp;
    }

    public void SetDisactivePosition()
    {
        IsActive = false;
        EndDate=DateTime.Now;
        ExpireDate=DateTime.Now;
    }

    //[ForeignKey("UserId")]
    //public virtual AppUser User { get; set; }
    //[ForeignKey("PositionId")]
    //public virtual Position Position { get; set; }
}