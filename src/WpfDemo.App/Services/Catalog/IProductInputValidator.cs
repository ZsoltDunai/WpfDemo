namespace WpfDemo.App.Services.Catalog;

public interface IProductInputValidator
{
    bool TryParse(string nameRaw, string priceRaw, out string name, out decimal price, out string errorMessage);
}
