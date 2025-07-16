public class GetAllContactsRequests
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    public string? FirstNameContains { get; set; }
    public string? LastNameContains { get; set; }
    public string? EmailContains { get; set; }
    public string? PhoneContains { get; set; }
}