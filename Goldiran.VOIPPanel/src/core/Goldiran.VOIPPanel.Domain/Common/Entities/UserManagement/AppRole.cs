using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;

namespace Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
public class AppRole : IdentityRole<long>, IAuditableEntity
{
    public Guid Guid { get; set; }
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public long? CreatedPositionId { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim> Claims { get; set; }
}
