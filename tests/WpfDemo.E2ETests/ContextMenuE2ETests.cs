using FlaUI.Core.Tools;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[Collection("WpfDemo")]
public class ContextMenuE2ETests
{
    [Fact]
    public void Remove_product_via_context_menu()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        var mugItem = WindowTestHelpers.RequireListItem(catalogWindow, "ProductListBox", "Mug - $12.00");
        mugItem.Select();
        WindowTestHelpers.InvokeContextMenuItem(fixture.Automation, mugItem, "RemoveProductMenuItem");
        catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        Retry.WhileTrue(
            () =>
            {
                var items = WindowTestHelpers.TryGetListItemTexts(catalogWindow, "ProductListBox");
                return items is not null && items.Contains("Mug - $12.00");
            },
            TimeSpan.FromSeconds(5));

        Assert.DoesNotContain("Mug - $12.00", WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox"));
        Assert.Equal("Removed Mug.", WindowTestHelpers.GetTextBoxValue(catalogWindow, "CatalogStatusTextBox"));
    }

    [Fact]
    public void Duplicate_product_via_context_menu()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        var coffeeItem = WindowTestHelpers.RequireListItem(catalogWindow, "ProductListBox", "Coffee - $4.50");
        coffeeItem.Select();
        WindowTestHelpers.InvokeContextMenuItem(fixture.Automation, coffeeItem, "DuplicateProductMenuItem");
        catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        Retry.WhileFalse(
            () =>
            {
                var items = WindowTestHelpers.TryGetListItemTexts(catalogWindow, "ProductListBox");
                return items is not null && items.Contains("Coffee (copy) - $4.50");
            },
            TimeSpan.FromSeconds(5));

        Assert.Contains("Coffee (copy) - $4.50", WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox"));
        Assert.Equal("Duplicated Coffee.", WindowTestHelpers.GetTextBoxValue(catalogWindow, "CatalogStatusTextBox"));
    }

    [Fact]
    public void Mark_featured_via_context_menu_updates_list_item()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenCatalogButton");
        var catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        var coffeeItem = WindowTestHelpers.RequireListItem(catalogWindow, "ProductListBox", "Coffee - $4.50");
        coffeeItem.Select();
        WindowTestHelpers.InvokeContextMenuItem(fixture.Automation, coffeeItem, "MarkFeaturedMenuItem");
        catalogWindow = fixture.WaitForWindow("Product Catalog");
        catalogWindow.Focus();

        Retry.WhileFalse(
            () =>
            {
                var items = WindowTestHelpers.TryGetListItemTexts(catalogWindow, "ProductListBox");
                return items is not null && items.Contains("[Featured] Coffee - $4.50");
            },
            TimeSpan.FromSeconds(5));

        Assert.Contains("[Featured] Coffee - $4.50", WindowTestHelpers.GetListItemTexts(catalogWindow, "ProductListBox"));
        Assert.Equal("Coffee marked as featured.", WindowTestHelpers.GetTextBoxValue(catalogWindow, "CatalogStatusTextBox"));
    }
}
