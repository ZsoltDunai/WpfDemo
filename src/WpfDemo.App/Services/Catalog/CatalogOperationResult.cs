using WpfDemo.App.Models;

namespace WpfDemo.App.Services.Catalog;

public sealed class CatalogOperationResult
{
    public bool Succeeded { get; init; }

    public string Message { get; init; } = string.Empty;

    public Product? Product { get; init; }

    public static CatalogOperationResult Success(string message, Product? product = null)
    {
        return new CatalogOperationResult
        {
            Succeeded = true,
            Message = message,
            Product = product,
        };
    }

    public static CatalogOperationResult Failure(string message)
    {
        return new CatalogOperationResult
        {
            Succeeded = false,
            Message = message,
        };
    }
}
