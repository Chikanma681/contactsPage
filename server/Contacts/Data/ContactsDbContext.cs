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
}