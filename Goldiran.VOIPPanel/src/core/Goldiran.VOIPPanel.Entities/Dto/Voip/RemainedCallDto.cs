using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class RemainedCallDto
{
    public int? TehranRemainedCount { get; set; }
    public int? ProvinceRemainedCount { get; set; }
    public int? TotalRemainedCount { get; set; }
    public int? TotalCount { get; set; }


}
