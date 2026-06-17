using System.Collections.ObjectModel;
using WpfDemo.App.Models;
using WpfDemo.App.Ui;

namespace WpfDemo.App.Services.Catalog;

public sealed class ProductCatalogService : IProductCatalogService
{
    public ObservableCollection<Product> Products { get; } = CreateDefaultProducts();

    public int Count => Products.Count;

    public CatalogOperationResult AddProduct(string name, decimal price)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
        };

        Products.Add(product);
        return CatalogOperationResult.Success($"Added {product.DisplayText}.", product);
    }

    public CatalogOperationResult MarkAsFeatured(Product product)
    {
        product.IsFeatured = true;
        return CatalogOperationResult.Success($"{product.Name} marked as featured.", product);
    }

    public CatalogOperationResult Duplicate(Product product)
    {
        var duplicate = product.Clone();
        duplicate.Name = $"{product.Name}{AppMessages.ProductCopySuffix}";

        Products.Add(duplicate);
        return CatalogOperationResult.Success($"Duplicated {product.Name}.", duplicate);
    }

    public CatalogOperationResult Remove(Product product)
    {
        Products.Remove(product);
        return CatalogOperationResult.Success($"Removed {product.Name}.");
    }

    private static ObservableCollection<Product> CreateDefaultProducts()
    {
        return
        [
            new Product { Name = "Coffee", Price = 4.50m },
            new Product { Name = "Mug", Price = 12.00m },
        ];
    }
}
