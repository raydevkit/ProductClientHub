using FluentValidation;
using ProductClientHub.App.ViewModels.Pages.Dashboard;

namespace ProductClientHub.App.Validation;

public class DashboardCreateClientValidator : AbstractValidator<DashboardViewModel>
{
    // Same rules as API RequestClientValidator
    public DashboardCreateClientValidator()
    {
        RuleFor(x => x.NewClient.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.NewClient.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.NewClient.Email)
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.NewClient.CodiceFiscale)
            .NotEmpty().WithMessage("Codice Fiscale is required.")
            .Length(16).WithMessage("Codice Fiscale must be exactly 16 characters.")
            .Matches(@"^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$")
            .WithMessage("Please check your Codice Fiscale");
    }
}

public class ClientItemEditValidator : AbstractValidator<ClientItemViewModel>
{
    public ClientItemEditValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.CodiceFiscale)
            .NotEmpty().WithMessage("Codice Fiscale is required.")
            .Length(16).WithMessage("Codice Fiscale must be exactly 16 characters.")
            .Matches(@"^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$")
            .WithMessage("Please check your Codice Fiscale");
    }
}

public class ClientItemCreateProductValidator : AbstractValidator<ClientItemViewModel>
{
    public ClientItemCreateProductValidator()
    {
        RuleFor(x => x.NewProductName)
            .NotEmpty().WithMessage("The product name must be provided.")
            .MaximumLength(100).WithMessage("The product name must not exceed 100 characters.");

        RuleFor(x => x.NewProductBrand)
            .NotEmpty().WithMessage("The product brand must be provided.")
            .MaximumLength(50).WithMessage("The product brand must not exceed 50 characters.");

        RuleFor(x => x.NewProductPrice)
            .GreaterThan(0).WithMessage("The product price must be greater than zero.");
    }
}
