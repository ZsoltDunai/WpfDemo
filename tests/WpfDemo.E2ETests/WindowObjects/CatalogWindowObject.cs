using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class CatalogWindowObject : WindowObjectBase
{
    public CatalogWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string Status => GetTextBoxValue("CatalogStatusTextBox");

    public IReadOnlyList<string> Products => GetListItemTexts("ProductListBox");

    public CatalogWindowObject AddProduct(string name, string price)
    {
        Focus();
        SetTextBoxValue("ProductNameTextBox", name);
        SetTextBoxValue("ProductPriceTextBox", price);
        ClickButton("AddProductButton");
        return this;
    }

    public CatalogWindowObject AddProductWithoutName()
    {
        Focus();
        SetTextBoxValue("ProductNameTextBox", string.Empty);
        ClickButton("AddProductButton");
        return this;
    }

    public CatalogWindowObject WaitUntilProductAppears(string productText, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);

        Retry.WhileFalse(
            () => TryGetListItemTexts("ProductListBox")?.Contains(productText) == true,
            timeout.Value);

        return this;
    }

    public CatalogWindowObject WaitUntilProductCount(int count, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);

        Retry.WhileFalse(
            () => TryGetListItemTexts("ProductListBox")?.Count == count,
            timeout.Value);

        return this;
    }

    public CatalogWindowObject RemoveProductViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, "RemoveProductMenuItem");
        return Refresh();
    }

    public CatalogWindowObject DuplicateProductViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, "DuplicateProductMenuItem");
        return Refresh();
    }

    public CatalogWindowObject MarkFeaturedViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, "MarkFeaturedMenuItem");
        return Refresh();
    }

    public CatalogWindowObject WaitUntilProductRemoved(string productText, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(5);

        Retry.WhileTrue(
            () => TryGetListItemTexts("ProductListBox")?.Contains(productText) == true,
            timeout.Value);

        return this;
    }

    public CatalogWindowObject WaitUntilStatus(string expectedStatus, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);

        Retry.WhileFalse(
            () => Status == expectedStatus,
            timeout.Value);

        return this;
    }

    private void InvokeProductContextMenu(string productText, string menuItemAutomationId)
    {
        Focus();
        var item = RequireListItem("ProductListBox", productText);
        item.Select();
        InvokeContextMenuItem(item, menuItemAutomationId);
    }

    private CatalogWindowObject Refresh()
    {
        return new CatalogWindowObject(Session.WaitForWindow("Product Catalog"), Session);
    }
}
