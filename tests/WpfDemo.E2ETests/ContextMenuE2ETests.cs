using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class ContextMenuE2ETests
{
    [Test]
    public void Remove_product_via_context_menu()
    {
        using var session = WpfDemoAppSession.Launch();
        var catalog = session.OpenMainWindow().OpenCatalog();

        catalog
            .RemoveProductViaContextMenu("Mug - $12.00")
            .WaitUntilProductRemoved("Mug - $12.00");

        Assert.That(catalog.Products, Does.Not.Contain("Mug - $12.00"));
        Assert.That(catalog.Status, Is.EqualTo("Removed Mug."));
    }

    [Test]
    public void Duplicate_product_via_context_menu()
    {
        using var session = WpfDemoAppSession.Launch();
        var catalog = session.OpenMainWindow().OpenCatalog();

        catalog
            .DuplicateProductViaContextMenu("Coffee - $4.50")
            .WaitUntilProductAppears("Coffee (copy) - $4.50", TimeSpan.FromSeconds(5));

        Assert.That(catalog.Products, Does.Contain("Coffee (copy) - $4.50"));
        Assert.That(catalog.Status, Is.EqualTo("Duplicated Coffee."));
    }

    [Test]
    public void Mark_featured_via_context_menu_updates_list_item()
    {
        using var session = WpfDemoAppSession.Launch();
        var catalog = session.OpenMainWindow().OpenCatalog();

        catalog
            .MarkFeaturedViaContextMenu("Coffee - $4.50")
            .WaitUntilProductAppears("[Featured] Coffee - $4.50", TimeSpan.FromSeconds(5))
            .WaitUntilStatus("Coffee marked as featured.");

        Assert.That(catalog.Products, Does.Contain("[Featured] Coffee - $4.50"));
        Assert.That(catalog.Status, Is.EqualTo("Coffee marked as featured."));
    }
}
