using System.Windows;
using System.Windows.Automation;
using WpfDemo.App.Models;
using WpfDemo.App.Services.Catalog;
using WpfDemo.App.Ui;

namespace WpfDemo.App;

public partial class CatalogWindow : Window
{
    private readonly IProductCatalogService _productCatalog;
    private readonly IProductInputValidator _productInputValidator;

    public CatalogWindow(
        IProductCatalogService productCatalog,
        IProductInputValidator productInputValidator)
    {
        _productCatalog = productCatalog;
        _productInputValidator = productInputValidator;

        InitializeComponent();
        AutomationProperties.SetAutomationId(this, AutomationIds.CatalogWindow);
        ProductListBox.ItemsSource = _productCatalog.Products;
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_productInputValidator.TryParse(
                ProductNameTextBox.Text,
                ProductPriceTextBox.Text,
                out var name,
                out var price,
                out var errorMessage))
        {
            ShowStatus(errorMessage);
            return;
        }

        var result = _productCatalog.AddProduct(name, price);
        ApplyResult(result, resetFormOnSuccess: true);
    }

    private void MarkFeaturedMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedProduct(out var product))
        {
            return;
        }

        var result = _productCatalog.MarkAsFeatured(product!);
        ProductListBox.Items.Refresh();
        ApplyResult(result);
    }

    private void DuplicateProductMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedProduct(out var product))
        {
            return;
        }

        var result = _productCatalog.Duplicate(product!);
        ApplyResult(result);
    }

    private void RemoveProductMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedProduct(out var product))
        {
            return;
        }

        var result = _productCatalog.Remove(product!);
        ApplyResult(result);
    }

    private void ApplyResult(CatalogOperationResult result, bool resetFormOnSuccess = false)
    {
        ShowStatus(result.Message);

        if (!result.Succeeded)
        {
            return;
        }

        if (result.Product is not null)
        {
            ProductListBox.SelectedItem = result.Product;
        }

        if (resetFormOnSuccess)
        {
            ResetProductForm();
        }
    }

    private void ResetProductForm()
    {
        ProductNameTextBox.Clear();
        ProductPriceTextBox.Text = AppMessages.DefaultProductPrice;
    }

    private bool TryGetSelectedProduct(out Product? product)
    {
        if (ProductListBox.SelectedItem is Product selectedProduct)
        {
            product = selectedProduct;
            return true;
        }

        product = null;
        ShowStatus(AppMessages.ProductSelectionRequired);
        return false;
    }

    private void ShowStatus(string message)
    {
        CatalogStatusTextBox.Text = message;
    }
}
