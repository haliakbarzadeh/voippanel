using Goldiran.VOIPPanel.Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class AppUser : IdentityUser<long>, IBaseQueryEntity
{
    public Guid Guid { get; set; }
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public long? CreatedPositionId { get; set; }
    public string PersianFullName { get; set; }
    public string? LatinFullName { get; set; }
    public string NationalCode { get; set; }
    public string? PersonalCode { get; set; }
    public UserType? UserType { get; set; }
    //public string? Mobile { get; set; }
    public bool IsActive { get; set; }
    //public virtual ICollection<AppUserUsedPassword> UserUsedPasswords { get; set; }
    public virtual ICollection<AppUserToken> UserTokens { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppUserLogin> Logins { get; set; }
    public virtual ICollection<AppUserClaim> Claims { get; set; }
    public virtual ICollection<UserPosition> UserPositions { get; set; }
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(PersianFullName)?string.Empty:PersianFullName;
    }
}
