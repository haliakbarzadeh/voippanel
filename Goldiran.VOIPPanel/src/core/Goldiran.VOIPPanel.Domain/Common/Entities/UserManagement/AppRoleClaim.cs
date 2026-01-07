//using Goldiran.VOIPPanel.Domain.Common.BaseModel;
using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
public class AppRoleClaim : IdentityRoleClaim<long>, IAuditableEntity
{
    public Guid Guid { get; set; }
    [ForeignKey("RoleId")]
    public virtual AppRole Role { get; set; }
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public long? CreatedPositionId { get; set; }
}
