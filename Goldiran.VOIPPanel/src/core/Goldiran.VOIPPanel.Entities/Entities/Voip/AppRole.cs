using Microsoft.AspNetCore.Identity;
using Voip.Framework.Domain;


namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class AppRole : IdentityRole<long>, IBaseQueryEntity
{
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    //public Guid Guid { get; set; }
    public long? CreatedPositionId { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim> Claims { get; set; }
}