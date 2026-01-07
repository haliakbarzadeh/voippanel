using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class AppUserRole : IdentityUserRole<long>, IBaseQueryEntity
{
    //public bool IsDeleted { get; set; }
    public DateTime? Created { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public long? CreatedPositionId { get; set; }
    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }
    [ForeignKey("RoleId")]
    public virtual AppRole Role { get; set; }
}