using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This aggregate root is responsible for managing the lifecycle of a sale,
/// including items, discounts, totals, and cancellation rules.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the unique sale number.
    /// Used for business identification and auditing.
    /// </summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Gets the external identifier of the customer.
    /// This value references another domain without a direct relationship.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the customer name at the time of the sale.
    /// This value is denormalized to preserve historical accuracy.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the external identifier of the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the branch name at the time of the sale.
    /// </summary>
    public string BranchName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the total amount of the sale.
    /// This value is automatically calculated based on sale items.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Indicates whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Internal collection of sale items.
    /// </summary>
    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// Gets the read-only collection of sale items.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class for EF Core.
    /// </summary>
    protected Sale() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="saleNumber">Unique sale number.</param>
    /// <param name="customerId">External customer identifier.</param>
    /// <param name="customerName">Customer name at the time of the sale.</param>
    /// <param name="branchId">External branch identifier.</param>
    /// <param name="branchName">Branch name at the time of the sale.</param>
    public Sale(
        string saleNumber,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        Number = saleNumber;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;

        Date = DateTime.UtcNow;
        IsCancelled = false;
        TotalAmount = 0;
    }

    /// <summary>
    /// Adds a new item to the sale.
    /// Applies business rules related to quantity limits and discount tiers.
    /// </summary>
    /// <param name="productId">External product identifier.</param>
    /// <param name="productName">Product name at the time of the sale.</param>
    /// <param name="quantity">Quantity of the product.</param>
    /// <param name="unitPrice">Unit price of the product.</param>
    public void AddItem(
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        var item = new SaleItem(productId, productName, quantity, unitPrice);
        _items.Add(item);

        RecalculateTotal();
    }

    /// <summary>
    /// Cancels the entire sale and all associated items.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;

        foreach (var item in _items)
            item.Cancel();

        TotalAmount = 0;
    }

    /// <summary>
    /// Recalculates the total amount of the sale
    /// considering only non-cancelled items.
    /// </summary>
    private void RecalculateTotal()
    {
        TotalAmount = _items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalAmount);
    }
}
