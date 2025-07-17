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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {
        Contact? contacts = await _dbContext.Contacts.FindAsync(id);

        if (contacts == null)
        {
            return NotFound();
        }

        return Ok(contacts);

    }
    

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest contact)
    {
        if (contact == null)
        {
            return BadRequest("Contact cannot be null.");
        }

        var newContact = new Contact
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Phone = contact.Phone,
            Address = contact.Address,
            City = contact.City,
            State = contact.State,
            ZipCode = contact.ZipCode,
            Country = contact.Country
        };

        _dbContext.Contacts.Add(newContact);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = newContact.Id }, contact);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] UpdateContactRequest contact)
    {

        if (contact == null)
        {
            return BadRequest("Contact cannot be null.");
        }


        Contact? existingContact = await _dbContext.Contacts.FindAsync(id);
        if (existingContact == null)
        {
            return NotFound();
        }

        existingContact.FirstName = contact.FirstName ?? existingContact.FirstName;
        existingContact.LastName = contact.LastName ?? existingContact.LastName;
        existingContact.Email = contact.Email ?? existingContact.Email;
        existingContact.Phone = contact.Phone ?? existingContact.Phone;
        existingContact.Address = contact.Address ?? existingContact.Address;
        existingContact.City = contact.City ?? existingContact.City;
        existingContact.State = contact.State ?? existingContact.State;
        existingContact.ZipCode = contact.ZipCode ?? existingContact.ZipCode;
        existingContact.Country = contact.Country ?? existingContact.Country;
        
        _dbContext.Contacts.Update(existingContact);
        
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> RemoveContact([FromRoute] int id)
    {
        Contact? existingContact = await _dbContext.Contacts.FindAsync(id);
        if (existingContact == null)
        {
            return NotFound();
        }

        _dbContext.Contacts.Remove(existingContact);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    
}