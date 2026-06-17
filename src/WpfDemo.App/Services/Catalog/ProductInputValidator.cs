using System.Globalization;
using WpfDemo.App.Ui;

namespace WpfDemo.App.Services.Catalog;

public sealed class ProductInputValidator : IProductInputValidator
{
    public bool TryParse(string nameRaw, string priceRaw, out string name, out decimal price, out string errorMessage)
    {
        name = nameRaw.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            price = default;
            errorMessage = AppMessages.ProductNameRequired;
            return false;
        }

        if (!TryParsePrice(priceRaw, out price))
        {
            errorMessage = AppMessages.ProductPriceInvalid;
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool TryParsePrice(string rawPrice, out decimal price)
    {
        return decimal.TryParse(
            rawPrice.Trim(),
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out price) && price >= 0;
    }
}
