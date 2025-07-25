using ContactsPage.Data;


// contains seeding logic when the application starts
public static class SeedData
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ContactsDbContext>();

        if(!context.Contacts.Any())
        {
            context.Contacts.AddRange(
                new Contact
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "janedoe@gmail.com",
                    Phone = "123-456-7890",
                    Address = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "12345",
                    Country = "USA"
                }, 
                new Contact
                {
                    FirstName = "James",
                    LastName = "Doe",
                    Email = "jamesdoe@gmail.com",
                    Phone = "123-456-7890",
                    Address = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "12345",
                    Country = "USA"
                }
            );
            context.SaveChanges();
        }
    }
}