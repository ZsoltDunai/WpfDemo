using System.Collections.ObjectModel;
using WpfDemo.App.Models;

namespace WpfDemo.App.Services;

public sealed class ProductCatalogService : IProductCatalogService
{
    public ObservableCollection<Product> Products { get; } =
    [
        new Product { Name = "Coffee", Price = 4.50m },
        new Product { Name = "Mug", Price = 12.00m },
    ];

    public int Count => Products.Count;

    public void Add(Product product) => Products.Add(product);

    public void Remove(Product product) => Products.Remove(product);
}
