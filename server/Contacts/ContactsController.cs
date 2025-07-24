using ContactsPage.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

public class ContactsController: BaseController
{
    private readonly ContactsDbContext _dbContext;
    private readonly ILogger<ContactsController> _logger;
    private readonly IValidator<CreateContactRequest> _createValidator;
    private readonly IValidator<UpdateContactRequest> _updateValidator;

    public ContactsController(
        ContactsDbContext dbContext, 
        ILogger<ContactsController> logger,
        IValidator<CreateContactRequest> createValidator,
        IValidator<UpdateContactRequest> updateValidator
        )
    {
        _dbContext = dbContext;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    
    /// <summary>
    /// Gets all contacts with optional filtering and pagination
    /// </summary>
    /// <param name="request">Query parameters for filtering and pagination</param>
    /// <returns>List of contacts</returns>
    /// <response code="200">Returns the list of contacts</response>
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
        return Ok(contacts.Select(ContactToGetContactResponse));
    }

    /// <summary>
    /// Gets a specific contact by ID
    /// </summary>
    /// <param name="id">The contact ID</param>
    /// <returns>Contact details</returns>
    /// <response code="200">Returns the contact</response>
    /// <response code="404">Contact not found</response>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {
        Contact? contacts = await _dbContext.Contacts.FindAsync(id);

        if (contacts == null)
        {
            return NotFound();
        }

        var response = ContactToGetContactResponse(contacts);
        return Ok(response);

    }
    
    /// <summary>
    /// Creates a new contact
    /// </summary>
    /// <param name="contact">Contact details to create</param>
    /// <returns>Created contact</returns>
    /// <response code="201">Contact created successfully</response>
    /// <response code="400">Invalid contact data</response>
    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest contact)
    {
        if (contact == null)
        {
            return BadRequest("Contact cannot be null.");
        }

        var validationResult = await _createValidator.ValidateAsync(contact);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(ModelState);
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

    /// <summary>
    /// Updates an existing contact
    /// </summary>
    /// <param name="id">Contact ID to update</param>
    /// <param name="contact">Updated contact details</param>
    /// <returns>No content</returns>
    /// <response code="204">Contact updated successfully</response>
    /// <response code="400">Invalid contact data</response>
    /// <response code="404">Contact not found</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] UpdateContactRequest contact)
    {
        _logger.LogInformation("Updating contact with ID: {Id}", id);
        if (contact == null)
        {
            _logger.LogWarning("Contact with ID {Id} is not found.", id);
            return BadRequest("Contact cannot be null.");
        }
        _logger.LogInformation("Validating contact with ID: {Id}", id);
        var validationResult = await _updateValidator.ValidateAsync(contact);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(ModelState);
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
         _logger.LogInformation("Employee with ID: {EmployeeId} successfully updated", id);
        return NoContent();
    }

    /// <summary>
    /// Deletes a contact
    /// </summary>
    /// <param name="id">Contact ID to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">Contact deleted successfully</response>
    /// <response code="404">Contact not found</response>
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

    private static GetContactResponse ContactToGetContactResponse(Contact contact)
    {
        return new GetContactResponse
        {
            Id = contact.Id,
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
    }
    

}