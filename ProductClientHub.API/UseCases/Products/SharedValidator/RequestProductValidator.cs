using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.UseCases.Products.SharedValidator
{
    public class RequestProductValidator : AbstractValidator<RequestProductJson>
    {
        public RequestProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("The product name must be provided.")
                .MaximumLength(100).WithMessage("The product name must not exceed 100 characters.");
            RuleFor(product => product.Brand)
                .NotEmpty().WithMessage("The product brand must be provided.")
                .MaximumLength(50).WithMessage("The product brand must not exceed 50 characters.");
            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("The product price must be greater than zero.");
        }
    }
}
