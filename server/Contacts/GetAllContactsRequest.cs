namespace ContactsPage.Data;

public class GetAllContactsRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    public string? FirstNameContains { get; set; }
    public string? LastNameContains { get; set; }
    public string? EmailContains { get; set; }
    public string? PhoneContains { get; set; }
}

public class GetContactResponse
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}