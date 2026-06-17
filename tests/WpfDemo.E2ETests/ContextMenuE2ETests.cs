using NUnit.Framework;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class ContextMenuE2ETests : E2ETestBase
{
    [Test]
    public void Remove_product_via_context_menu()
    {
        var catalog = Main.OpenCatalog();

        catalog
            .RemoveProductViaContextMenu(TestProducts.Mug)
            .WaitUntilProductRemoved(TestProducts.Mug);

        Assert.That(catalog.Products, Does.Not.Contain(TestProducts.Mug));
        Assert.That(catalog.Status, Is.EqualTo("Removed Mug."));
    }

    [Test]
    public void Duplicate_product_via_context_menu()
    {
        var catalog = Main.OpenCatalog();

        catalog
            .DuplicateProductViaContextMenu(TestProducts.Coffee)
            .WaitUntilProductAppears(TestProducts.CoffeeCopy, UiTimeouts.ContextMenu);

        Assert.That(catalog.Products, Does.Contain(TestProducts.CoffeeCopy));
        Assert.That(catalog.Status, Is.EqualTo("Duplicated Coffee."));
    }

    [Test]
    public void Mark_featured_via_context_menu_updates_list_item()
    {
        var catalog = Main.OpenCatalog();

        catalog
            .MarkFeaturedViaContextMenu(TestProducts.Coffee)
            .WaitUntilProductAppears(TestProducts.FeaturedCoffee, UiTimeouts.ContextMenu)
            .WaitUntilStatus("Coffee marked as featured.");

        Assert.That(catalog.Products, Does.Contain(TestProducts.FeaturedCoffee));
        Assert.That(catalog.Status, Is.EqualTo("Coffee marked as featured."));
    }
}
