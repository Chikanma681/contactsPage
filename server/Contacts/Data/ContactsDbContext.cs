using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace ContactsPage.Data;

public class ContactsDbContext: DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options): base(options)
    {
    }
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<ContactCategory> ContactCategories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .HasOne(c=>c.Category)
            .WithMany(cc => cc.Contacts)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<AuditFields>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "TheCreateUser"; // Replace with actual user context
                entry.Entity.CreatedOn = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = "TheUpdateUser"; // Replace with actual user context
                entry.Entity.LastModifiedOn = DateTime.UtcNow;
            }
        }
    }
}