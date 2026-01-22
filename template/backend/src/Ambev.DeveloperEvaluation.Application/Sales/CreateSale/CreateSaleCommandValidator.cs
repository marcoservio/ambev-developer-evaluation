using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommandValidator that defines validation rules for user creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Number: Must be maximum 50 characters and not empty
    /// - Date: Must not be empty
    /// - CustomerId: Must not be empty
    /// - CustomerName: Must be maximum 100 characters and not empty
    /// - BranchId: Must not be empty
    /// - BranchName: Must be maximum 100 characters and not empty
    /// - Items: Must contain at least one item, each item validated by CreateSaleItemCommandValidator
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Number).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Items).NotEmpty().WithMessage("Sale must contain at least one item.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemCommandValidator());
    }
}
