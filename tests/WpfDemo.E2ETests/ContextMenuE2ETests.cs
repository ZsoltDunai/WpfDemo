using NUnit.Framework;

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
            .RemoveProductViaContextMenu("Mug - $12.00")
            .WaitUntilProductRemoved("Mug - $12.00");

        Assert.That(catalog.Products, Does.Not.Contain("Mug - $12.00"));
        Assert.That(catalog.Status, Is.EqualTo("Removed Mug."));
    }

    [Test]
    public void Duplicate_product_via_context_menu()
    {
        var catalog = Main.OpenCatalog();

        catalog
            .DuplicateProductViaContextMenu("Coffee - $4.50")
            .WaitUntilProductAppears("Coffee (copy) - $4.50", TimeSpan.FromSeconds(5));

        Assert.That(catalog.Products, Does.Contain("Coffee (copy) - $4.50"));
        Assert.That(catalog.Status, Is.EqualTo("Duplicated Coffee."));
    }

    [Test]
    public void Mark_featured_via_context_menu_updates_list_item()
    {
        var catalog = Main.OpenCatalog();

        catalog
            .MarkFeaturedViaContextMenu("Coffee - $4.50")
            .WaitUntilProductAppears("[Featured] Coffee - $4.50", TimeSpan.FromSeconds(5))
            .WaitUntilStatus("Coffee marked as featured.");

        Assert.That(catalog.Products, Does.Contain("[Featured] Coffee - $4.50"));
        Assert.That(catalog.Status, Is.EqualTo("Coffee marked as featured."));
    }
}
