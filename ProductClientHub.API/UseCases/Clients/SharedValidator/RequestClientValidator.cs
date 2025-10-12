using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.UseCases.Clients.SharedValidator
{
    public class RequestClientValidator : AbstractValidator<RequestClientJson>
    {
        public RequestClientValidator()
        {
            RuleFor(client => client.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(client => client.Email).EmailAddress().WithMessage("Invalid email format.");
            RuleFor(client => client.CodiceFiscale)
                .NotEmpty().WithMessage("Codice Fiscale is required.")
                .Length(16).WithMessage("Codice Fiscale must be exactly 16 characters.")
                .Matches(@"^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$")
                .WithMessage("Please check your Codice Fiscale");
            RuleFor(client => client.LastName).NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
