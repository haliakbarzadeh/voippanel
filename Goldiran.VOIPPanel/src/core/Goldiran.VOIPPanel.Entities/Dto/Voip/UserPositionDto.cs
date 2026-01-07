using AutoMapper;
using Voip.Framework.Domain.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class UserPositionDto 
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PositionId { get; set; }
    public DateTime StartDate { get; set; }
    public string PersianStartDate { get { return StartDate.ConvertDateTimeToJalaliDateTime(); } }
    public DateTime? EndDate { get; set; }
    public string PersianEndDate { get { return EndDate!=null?((DateTime)EndDate).ConvertDateTimeToJalaliDateTime():string.Empty; } }
    public DateTime? ExpireDate { get; set; }
    public string PersianExpireDate { get { return ExpireDate != null ? ((DateTime)ExpireDate).ConvertDateTimeToJalaliDateTime() : string.Empty; } }
    public bool IsActive { get; set; }
    public bool IsContentAccess { get; set; }
    public string? Queues { get; set; }
    public string? Extension { get; set; }
    public string? ServerIp { get; set; }
    public string User { get; set; }
    public string UserExt { get { return $"{User}_{Extension}"; } }
    public string Position { get; set; }

}