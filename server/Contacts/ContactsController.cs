using ContactsPage.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ContactsController: BaseController
{
    private readonly ContactsDbContext _dbContext;

    public ContactsController(ContactsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContacts([FromQuery] GetAllContactsRequest request)
    {
        int page = ((request?.Page) > 0 ?  request?.Page: 1) ?? 0;
        int contactsPerPage = ((request?.RecordsPerPage) > 0 ? request?.RecordsPerPage : 100) ?? 100;


        IQueryable<Contact> query = _dbContext.Contacts
        .Skip((page - 1) * contactsPerPage)
        .Take(contactsPerPage);

        if (request != null)
        {
            if (!string.IsNullOrWhiteSpace(request.FirstNameContains))
            {
                query = query.Where(e => e.FirstName.Contains(request.FirstNameContains));
            }
            
            if (!string.IsNullOrWhiteSpace(request.LastNameContains))
            {
                query = query.Where(e => e.LastName.Contains(request.LastNameContains));
            }
        }

        var contacts = await query.ToArrayAsync();

        return Ok(contacts);
    }
}