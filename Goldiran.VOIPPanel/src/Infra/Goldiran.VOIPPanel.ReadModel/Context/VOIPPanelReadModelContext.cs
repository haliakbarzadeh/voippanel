using Voip.Framework.Domain.CommonServices.IServices;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.EFCore;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.QueryHandler.Common.Extensions;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Entities.Voip;

namespace Goldiran.VOIPPanel.QueryHandler;

public partial class VOIPPanelReadModelContext : DbContext, IReadModelContext
{
    public VOIPPanelReadModelContext(DbContextOptions<VOIPPanelReadModelContext> options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.LazyLoadingEnabled = false;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddCustomGoldiranMappings();

        builder.ApplyUtcDateTimeConverter();
    }
    public void ExecuteSqlCommand(string query, params object[] parameters)
    {
        Database.ExecuteSqlRaw(query, parameters);
    }
    #region DBSet Region
    public virtual DbSet<AppRole> Role { get; set; }
    public virtual DbSet<AppRoleClaim> RoleClaims { get; set; }
    public virtual DbSet<AppUser> User { get; set; }
    public virtual DbSet<AppUserClaim> AppUserClaim { get; set; }
    public virtual DbSet<AppUserRole> UserRoles { get; set; }
    public virtual DbSet<AppUserLogin> UserLogins { get; set; }
    public virtual DbSet<AppUserToken> AppUserTokens { get; set; }
    public virtual DbSet<AppUserUsedPassword> AppUserUsedPassword { get; set; }
    public virtual DbSet<LoginHistory> LoginHistory { get; set; }
    public virtual DbSet<MonitoredPosition> MonitoredPosition { get; set; }
    public virtual DbSet<Position> Position { get; set; }
    public virtual DbSet<Token> Token { get; set; }
    public virtual DbSet<UserPosition> UserPosition { get; set; }
    public virtual DbSet<Goldiran.VOIPPanel.ReadModel.Entities.File> File { get; set; }
    public virtual DbSet<FileContent> FileContent { get; set; }
    public virtual DbSet<Queu> Queu { get; set; }
    public virtual DbSet<Operation> Operation { get; set; }
    public virtual DbSet<OperationSetting> OperationSetting { get; set; }
    public virtual DbSet<QueueLimitation> QueueLimitations { get; set; }
    public virtual DbSet<HourValue> HourValues { get; set; }
    public virtual DbSet<ContactDetail> ContactDetails { get; set; }
    public virtual DbSet<FlatDataJob> FlatDataJobs { get; set; }
    public virtual DbSet<TempFlatDataJob> TempFlatDataJobs { get; set; }
    public virtual DbSet<SoftPhoneEvent> SoftPhoneEvents { get; set; }

    #endregion
}
