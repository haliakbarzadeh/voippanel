using Goldiran.VOIPPanel.QueryHandler.Common.Extensions;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Extensions;
using Voip.Framework.EFCore;

namespace Goldiran.VOIPPanel.QueryHandler;

public partial class AsteriskReadModelContext : DbContext,IReadModelContext
{
    public AsteriskReadModelContext(DbContextOptions<AsteriskReadModelContext> options) : base(options)
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
    public virtual DbSet<AskCustomer> AskCustomers { get; set; }
    public virtual DbSet<CityName> CityNames { get; set; }
    public virtual DbSet<CustomerInfo> CustomerInfos { get; set; }
    public virtual DbSet<RegionName> RegionNames { get; set; }
    public virtual DbSet<SecureCall> SecureCalls { get; set; }
    public virtual DbSet<QueueLog> QueueLogs { get; set; }
    public virtual DbSet<AnsweringMachine> AnsweringMachines { get; set; }


    #endregion
}
