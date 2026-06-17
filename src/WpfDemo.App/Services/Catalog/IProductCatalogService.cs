using WpfDemo.App.Models;

namespace WpfDemo.App.Services.Catalog;

public interface IProductCatalogService : IProductCatalogReader
{
    CatalogOperationResult AddProduct(string name, decimal price);

    CatalogOperationResult MarkAsFeatured(Product product);

    CatalogOperationResult Duplicate(Product product);

    CatalogOperationResult Remove(Product product);
}
