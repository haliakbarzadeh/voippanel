using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class UserDto 
{
    public Guid Guid { get; set; }
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    [JsonIgnore]
    public int AccessFailedCount { get; set; }
    public string NationalCode { get; set; }
    public string PersianFullName { get; set; }
    public string? PersonalCode { get; set; }
    public string Name { get { return !string.IsNullOrEmpty(PersianFullName) ? PersianFullName : string.Empty; } }
    public string Title { get { return !string.IsNullOrEmpty(PersianFullName) ? PersianFullName : string.Empty; } }
    public string? LatinFullName { get; set; }
    public int OrganizationTypeId { get; set; }
    //[JsonIgnore]
    //public DepartmentType OrganizationType { get; set; }
    //public string? OrganizationTypeName { get { return OrganizationType.DepType; } }
    public string? NativeDepartmentName { get; set; }
    public string? NativePsitionName { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? ReportPath { get; set; }
    public bool IsActive { get; set; }
    public string PersianIsActive { get {return IsActive ? "فعال" : "غیر فعال"; } }
    public UserType? UserType { get; set; }
    public string UserTypeStr { get { return UserType != null ? UserType.Description() : string.Empty; } }

    public bool? IsComfired { get; set; }

    //public List<FileRowDto> AttachmentsFiles { get; set; } = new List<FileRowDto>();
    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<AppUser, UserDto>();
    //}
}