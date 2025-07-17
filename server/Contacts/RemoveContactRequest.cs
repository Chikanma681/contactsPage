namespace ContactsPage.Data;

public class RemoveContactRequest
{
    public int Id { get; set; }

    public RemoveContactRequest(int id)
    {
        Id = id;
    }
}