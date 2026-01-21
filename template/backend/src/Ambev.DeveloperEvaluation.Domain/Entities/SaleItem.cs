using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale.
/// Contains quantity-based discount rules and item-level cancellation logic.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the external product identifier.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the product name at the time of the sale.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied to the item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item after discount.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Indicates whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class for EF Core.
    /// </summary>
    protected SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class.
    /// Applies quantity-based discount rules.
    /// </summary>
    public SaleItem(
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice)
    {
        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;

        Discount = CalculateDiscount(quantity);
        TotalAmount = CalculateTotal();
        IsCancelled = false;
    }

    /// <summary>
    /// Cancels the sale item.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
        TotalAmount = 0;
    }

    /// <summary>
    /// Calculates the discount percentage based on item quantity.
    /// </summary>
    private static decimal CalculateDiscount(int quantity)
    {
        if (quantity >= 10)
            return 0.20m;

        if (quantity >= 4)
            return 0.10m;

        return 0;
    }

    /// <summary>
    /// Calculates the total amount for the item after applying the discount.
    /// </summary>
    private decimal CalculateTotal()
    {
        var gross = Quantity * UnitPrice;
        return gross - (gross * Discount);
    }
}
