using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : BaseEntityConfiguration<SaleItem>
{
    public override void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("SaleItems");

        builder.Property<Guid>("SaleId")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.ProductId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Discount)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.IsCancelled)
            .IsRequired();
    }
}