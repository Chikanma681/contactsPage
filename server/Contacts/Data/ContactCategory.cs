namespace ContactsPage.Data;

public class ContactCategory
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public string? Description { get; set; }

    public List<Contact> Contacts { get; set; } = new List<Contact>();

}