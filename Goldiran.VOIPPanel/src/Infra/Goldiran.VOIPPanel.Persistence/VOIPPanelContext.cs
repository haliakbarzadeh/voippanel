using Voip.Framework.Domain;
using Voip.Framework.Domain.CommonServices.IServices;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Data;
using Voip.Framework.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Voip.Framework.EFCore.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Persistence.Common.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System.Collections;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents;


namespace Goldiran.VOIPPanel.Persistence;

public class VOIPPanelContext : IdentityDbContext<AppUser, AppRole, long, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
        , IUnitOfWork
{
    #region Private property
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;
    public IDbContextTransaction CurrentTransaction { get; set; }
    //public const string DEFAULT_SCHEMA = "voip";
    #endregion

    #region DBSet Region
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppRoleClaim> AppRoleClaims { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppUserClaim> AppUserClaims { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<AppUserLogin> AppUserLogins { get; set; }
    public DbSet<AppUserToken> AppUserTokens { get; set; }
    //public DbSet<AppUserUsedPassword> AppUserUsedPasswords { get; set; }
    public DbSet<LoginHistory> LoginHistorys { get; set; }
    public DbSet<MonitoredPosition> MonitoredPositions { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<IntegrationEventLogEntry> IntegrationEventLogEntrys { get; set; }
    public DbSet<Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File> Files { get; set; }
    public DbSet<FileContent> FileContents { get; set; }
    public DbSet<Queu> Queus { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<OperationSetting> OperationSettings { get; set; }
    public DbSet<QueueLimitation> QueueLimitations { get; set; }
    public DbSet<HourValue> HourValues { get; set; }
    public DbSet<ContactDetail> ContactDetails { get; set; }
    public DbSet<FlatDataJob> FlatDataJobs { get; set; }
    public DbSet<TempFlatDataJob> TempFlatDataJobs { get; set; }
    public DbSet<SoftPhoneEvent> SoftPhoneEvents { get; set; }
    #endregion

    #region Override

    #endregion

    #region Constructor
    public VOIPPanelContext(DbContextOptions<VOIPPanelContext> options, IMediator mediator, ICurrentUserService currentUserService) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

    }
    #endregion
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddCustomGoldiranMappings();

        //builder.ApplyUtcDateTimeConverter();
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
    public override void Dispose()
    {
        CurrentTransaction?.Dispose();
        base.Dispose();
    }
    public int Commit()
    {
        var entityForSave = ChangeTracker
          .Entries()
          .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added || x.State == EntityState.Deleted)
          .ToList();
        int result = SaveChanges();
        return result;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Database.CreateExecutionStrategy();
    }

    public void ExecuteSqlRawCommand(string query, params object[] parameters)
    {
        Database.ExecuteSqlRaw(query, parameters);
    }
    public IDbContextTransaction GetCurrentTransaction() => CurrentTransaction;

    public bool HasActiveTransaction => CurrentTransaction != null;
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (CurrentTransaction != null) return null;

        CurrentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

        return CurrentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (CurrentTransaction == null) throw new ArgumentNullException(nameof(CurrentTransaction));
        if (CurrentTransaction != CurrentTransaction) throw new InvalidOperationException($"Transaction {CurrentTransaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await CurrentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            CurrentTransaction?.Rollback();
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        ChangeTracker.AutoDetectChangesEnabled = false;
        await _mediator.DispatchDomainEventsAsync(this);
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedPositionId = _currentUserService.PosId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }
        var result = await base.SaveChangesAsync(cancellationToken);
        ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.DetectChanges();

        ChangeTracker.AutoDetectChangesEnabled = false;
        _mediator.DispatchDomainEvents(this);
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedPositionId = _currentUserService.PosId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess);
        ChangeTracker.AutoDetectChangesEnabled = true;
        return result;
    }

    public DatabaseFacade GetDatabase() => Database;
}
