using System.Collections.ObjectModel;
using WpfDemo.App.Models;

namespace WpfDemo.App.Services.Catalog;

public interface IProductCatalogReader
{
    ObservableCollection<Product> Products { get; }

    int Count { get; }
}
