using Microsoft.EntityFrameworkCore;

namespace ContactsPage.Data;

public class ContactsDbContext: DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options): base(options)
    {
    }
    // The null! tells the compiler that even though Contacts is nullable,
    // we guarantee it will be initialized by EF Core when the context is created.
    // This prevents nullable reference warnings while maintaining type safety.
    public DbSet<Contact> Contacts { get; set; } = null!;
}