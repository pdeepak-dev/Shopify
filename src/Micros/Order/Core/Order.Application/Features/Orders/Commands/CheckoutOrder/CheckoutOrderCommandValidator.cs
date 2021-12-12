using FluentValidation;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{Email} is required.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("{Price} is required.")
                .GreaterThan(0).WithMessage("{Price} should be greater than zero.");
        }
    }
}