using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.Infrastructure;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class CatalogWindowObject : WindowObjectBase
{
    public CatalogWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string Status => GetTextBoxValue(AutomationIds.CatalogStatusTextBox);

    public IReadOnlyList<string> Products => GetListItemTexts(AutomationIds.ProductListBox);

    public CatalogWindowObject AddProduct(string name, string price)
    {
        Focus();
        SetTextBoxValue(AutomationIds.ProductNameTextBox, name);
        SetTextBoxValue(AutomationIds.ProductPriceTextBox, price);
        ClickButton(AutomationIds.AddProductButton);
        return this;
    }

    public CatalogWindowObject AddProductWithoutName()
    {
        Focus();
        SetTextBoxValue(AutomationIds.ProductNameTextBox, string.Empty);
        ClickButton(AutomationIds.AddProductButton);
        return this;
    }

    public CatalogWindowObject WaitUntilProductAppears(string productText, TimeSpan? timeout = null)
    {
        WaitUntil(
            () => TryGetListItemTexts(AutomationIds.ProductListBox)?.Contains(productText) == true,
            timeout);

        return this;
    }

    public CatalogWindowObject WaitUntilProductCount(int count, TimeSpan? timeout = null)
    {
        WaitUntil(
            () => TryGetListItemTexts(AutomationIds.ProductListBox)?.Count == count,
            timeout);

        return this;
    }

    public CatalogWindowObject RemoveProductViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, AutomationIds.RemoveProductMenuItem);
        return Refresh();
    }

    public CatalogWindowObject DuplicateProductViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, AutomationIds.DuplicateProductMenuItem);
        return Refresh();
    }

    public CatalogWindowObject MarkFeaturedViaContextMenu(string productText)
    {
        InvokeProductContextMenu(productText, AutomationIds.MarkFeaturedMenuItem);
        return Refresh();
    }

    public CatalogWindowObject WaitUntilProductRemoved(string productText, TimeSpan? timeout = null)
    {
        WaitUntil(
            () => TryGetListItemTexts(AutomationIds.ProductListBox)?.Contains(productText) != true,
            timeout ?? UiTimeouts.ContextMenu);

        return this;
    }

    public CatalogWindowObject WaitUntilStatus(string expectedStatus, TimeSpan? timeout = null)
    {
        WaitUntil(() => Status == expectedStatus, timeout);
        return this;
    }

    private void InvokeProductContextMenu(string productText, string menuItemAutomationId)
    {
        Focus();
        var item = RequireListItem(AutomationIds.ProductListBox, productText);
        item.Select();
        InvokeContextMenuItem(item, menuItemAutomationId);
    }

    private CatalogWindowObject Refresh()
    {
        return new CatalogWindowObject(Session.WaitForWindow(WindowTitles.Catalog), Session);
    }

    private static void WaitUntil(Func<bool> condition, TimeSpan? timeout)
    {
        Retry.WhileFalse(condition, timeout ?? UiTimeouts.Default);
    }
}
