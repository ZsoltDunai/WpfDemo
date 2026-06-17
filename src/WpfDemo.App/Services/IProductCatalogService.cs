using System.Collections.ObjectModel;
using WpfDemo.App.Models;

namespace WpfDemo.App.Services;

public interface IProductCatalogService
{
    ObservableCollection<Product> Products { get; }

    int Count { get; }

    void Add(Product product);

    void Remove(Product product);
}
