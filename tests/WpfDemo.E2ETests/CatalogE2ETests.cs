using FlaUI.Core.Tools;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[Collection("WpfDemo")]
public class CatalogE2ETests
{
    [Fact]
    public void Adding_product_updates_list_and_status()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        Assert.Equal(["Coffee - $4.50", "Mug - $12.00"], WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox"));

        WindowTestHelpers.RequireTextBox(catalogWindow, "ProductNameTextBox").Text = "Tea";
        WindowTestHelpers.RequireTextBox(catalogWindow, "ProductPriceTextBox").Text = "3.50";
        WindowTestHelpers.ClickButton(catalogWindow, "AddProductButton");

        Retry.WhileFalse(
            () => WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox").Contains("Tea - $3.50"),
            TimeSpan.FromSeconds(3));

        Assert.Contains("Tea - $3.50", WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox"));
        Assert.Equal("Added Tea - $3.50.", WindowTestHelpers.GetTextBoxValue(catalogWindow, "CatalogStatusTextBox"));
    }

    [Fact]
    public void Adding_product_without_name_shows_validation()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        WindowTestHelpers.RequireTextBox(catalogWindow, "ProductNameTextBox").Text = string.Empty;
        WindowTestHelpers.ClickButton(catalogWindow, "AddProductButton");

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(catalogWindow, "CatalogStatusTextBox") == "Enter a product name.",
            TimeSpan.FromSeconds(3));

        Assert.Equal(2, WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox").Count);
    }

    [Fact]
    public void Closing_catalog_updates_product_summary_on_main_window()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        Assert.Equal("Products in catalog: 2", WindowTestHelpers.GetTextBoxValue(mainWindow, "ProductSummaryTextBox"));

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        WindowTestHelpers.RequireTextBox(catalogWindow, "ProductNameTextBox").Text = "Notebook";
        WindowTestHelpers.RequireTextBox(catalogWindow, "ProductPriceTextBox").Text = "6.25";
        WindowTestHelpers.ClickButton(catalogWindow, "AddProductButton");

        Retry.WhileFalse(
            () => WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox").Count == 3,
            TimeSpan.FromSeconds(3));

        catalogWindow.Close();

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(mainWindow, "ProductSummaryTextBox") == "Products in catalog: 3",
            TimeSpan.FromSeconds(3));

        Assert.Equal("Products in catalog: 3", WindowTestHelpers.GetTextBoxValue(mainWindow, "ProductSummaryTextBox"));
    }
}
