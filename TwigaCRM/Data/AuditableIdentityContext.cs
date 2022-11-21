
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Enums;
using TwigaCRM.Models;

namespace J.E.Data
{
    public abstract class AuditableIdentityContext : IdentityDbContext<AppUser>
    {
        public AuditableIdentityContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Audit> AuditLogs { get; set; }
        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is TimeStamps && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((TimeStamps)entityEntry.Entity).UpdateAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((TimeStamps)entityEntry.Entity).CreateAt = DateTime.Now;
                    ((TimeStamps)entityEntry.Entity).UpdateAt = DateTime.Now;
                }
            }
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }
        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = TwigaCRM.Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = TwigaCRM.Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;    
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = TwigaCRM.Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
       
    }
}
