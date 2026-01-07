//using Goldiran.VOIPPanel.Domain.Common.BaseModel;
using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;


public class AppUserLogin : IdentityUserLogin<long>, IAuditableEntity
{
    public Guid Guid { get; set; }
    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public long? CreatedPositionId { get; set; }
    public long? CreatedCorporationId { get; set; }
    public long? LastModifiedPositionId { get; set; }
}
