using System.Globalization;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using WpfDemo.App.Models;

namespace WpfDemo.App;

public partial class CatalogWindow : Window
{
    public CatalogWindow()
    {
        InitializeComponent();
        AutomationProperties.SetAutomationId(this, "CatalogWindow");
        ProductListBox.ItemsSource = ProductCatalog.Products;
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        var name = ProductNameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            CatalogStatusTextBox.Text = "Enter a product name.";
            return;
        }

        if (!decimal.TryParse(ProductPriceTextBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var price)
            || price < 0)
        {
            CatalogStatusTextBox.Text = "Enter a valid price.";
            return;
        }

        var product = new Product
        {
            Name = name,
            Price = price,
        };

        ProductCatalog.Products.Add(product);
        ProductListBox.SelectedItem = product;
        ProductNameTextBox.Clear();
        ProductPriceTextBox.Text = "0.00";
        CatalogStatusTextBox.Text = $"Added {product.DisplayText}.";
    }

    private void MarkFeaturedMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (ProductListBox.SelectedItem is not Product product)
        {
            CatalogStatusTextBox.Text = "Select a product first.";
            return;
        }

        product.IsFeatured = true;
        ProductListBox.Items.Refresh();
        CatalogStatusTextBox.Text = $"{product.Name} marked as featured.";
    }

    private void DuplicateProductMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (ProductListBox.SelectedItem is not Product product)
        {
            CatalogStatusTextBox.Text = "Select a product first.";
            return;
        }

        var duplicate = product.Clone();
        duplicate.Name = $"{product.Name} (copy)";

        ProductCatalog.Products.Add(duplicate);
        ProductListBox.SelectedItem = duplicate;
        CatalogStatusTextBox.Text = $"Duplicated {product.Name}.";
    }

    private void RemoveProductMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (ProductListBox.SelectedItem is not Product product)
        {
            CatalogStatusTextBox.Text = "Select a product first.";
            return;
        }

        ProductCatalog.Products.Remove(product);
        CatalogStatusTextBox.Text = $"Removed {product.Name}.";
    }
}
