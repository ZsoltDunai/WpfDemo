using System.Collections.ObjectModel;
using WpfDemo.App.Models;

namespace WpfDemo.App;

public static class ProductCatalog
{
    public static ObservableCollection<Product> Products { get; } =
    [
        new Product { Name = "Coffee", Price = 4.50m },
        new Product { Name = "Mug", Price = 12.00m },
    ];

    public static int Count => Products.Count;
}
