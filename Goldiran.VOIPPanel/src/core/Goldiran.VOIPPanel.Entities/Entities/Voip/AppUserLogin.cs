using Voip.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.ReadModel.Entities
{

    public class AppUserLogin : IdentityUserLogin<long>, IBaseQueryEntity
    {
        //public Guid Guid { get; set; }
        public DateTime? Created { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? LastModifiedBy { get; set; }
        public long? CreatedPositionId { get; set; }
        //public long? CreatedCorporationId { get; set; }
        //public long? LastModifiedPositionId { get; set; }
        //[ForeignKey("UserId")]
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

    }
}