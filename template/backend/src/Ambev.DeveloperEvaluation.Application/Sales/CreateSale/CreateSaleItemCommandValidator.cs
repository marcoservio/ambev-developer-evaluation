using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleItemCommandValidator that defines validation rules for user creation command.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - ProductId: Must not be empty
    /// - ProductName: Must be maximum 100 characters and not empty
    /// - Quantity: Must be greater than 0 and less than or equal to 20
    /// - UnitPrice: Must be greater than 0
    /// </remarks>
    public CreateSaleItemCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Quantity).GreaterThan(0).LessThanOrEqualTo(20).WithMessage("It is not allowed to sell more than 20 identical items.");
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}
