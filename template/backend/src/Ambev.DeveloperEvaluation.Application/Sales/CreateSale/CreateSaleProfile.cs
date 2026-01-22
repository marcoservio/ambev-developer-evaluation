using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSale
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale operation
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ConstructUsing(src =>
                new Sale(
                    src.Number,
                    src.CustomerId,
                    src.CustomerName,
                    src.BranchId,
                    src.BranchName
                ))
            .ForMember(dest => dest.Items, opt => opt.Ignore())   // 👈 ISSO
            .AfterMap((src, dest) =>
            {
                if (src.Items == null) return;

                foreach (var item in src.Items)
                {
                    dest.AddItem(
                        item.ProductId,
                        item.ProductName,
                        item.Quantity,
                        item.UnitPrice
                    );
                }
            });

        CreateMap<CreateSaleItemCommand, SaleItem>()
            .ConstructUsing(src =>
                        new SaleItem(
                            src.ProductId,
                            src.ProductName,
                            src.Quantity,
                            src.UnitPrice
                        ));

        CreateMap<Sale, CreateSaleResult>();
    }
}
