using FluentValidation;
using ContactsPage.Data;

namespace ContactsPage.Contacts;

public class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
{
    public CreateContactRequestValidator()
    {
        RuleFor(x=>x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .Length(1, 50)
            .WithMessage("First name must be between 1 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .Length(1, 50)
            .WithMessage("Last name must be between 1 and 50 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please provide a valid email address")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^(\+\d{1,3}[- ]?)?\d{10}$")
            .WithMessage("Please provide a valid phone number")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.City)
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.State)
            .MaximumLength(50)
            .WithMessage("State cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.State));

        RuleFor(x => x.ZipCode)
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Please provide a valid ZIP code (e.g., 12345 or 12345-6789)")
            .When(x => !string.IsNullOrEmpty(x.ZipCode));

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .WithMessage("Country cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
} 