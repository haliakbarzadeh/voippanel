using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.ReadModel.Entities
{

    public class AppRoleClaim : IdentityRoleClaim<long>, IBaseQueryEntity
    {
        //public Guid Guid { get; set; }
        public DateTime? Created { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? LastModifiedBy { get; set; }
        public long? CreatedPositionId { get; set; }
        //public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual AppRole Role { get; set; }

    }
}