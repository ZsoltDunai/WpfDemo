using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class CatalogE2ETests
{
    [Test]
    public void Adding_product_updates_list_and_status()
    {
        using var session = WpfDemoAppSession.Launch();
        var catalog = session.OpenMainWindow().OpenCatalog();

        Assert.That(catalog.Products, Is.EqualTo(new[] { "Coffee - $4.50", "Mug - $12.00" }));

        catalog.AddProduct("Tea", "3.50").WaitUntilProductAppears("Tea - $3.50");

        Assert.That(catalog.Products, Does.Contain("Tea - $3.50"));
        Assert.That(catalog.Status, Is.EqualTo("Added Tea - $3.50."));
    }

    [Test]
    public void Adding_product_without_name_shows_validation()
    {
        using var session = WpfDemoAppSession.Launch();
        var catalog = session.OpenMainWindow().OpenCatalog();

        catalog.AddProductWithoutName().WaitUntilStatus("Enter a product name.");

        Assert.That(catalog.Products, Has.Count.EqualTo(2));
    }

    [Test]
    public void Closing_catalog_updates_product_summary_on_main_window()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        Assert.That(main.ProductSummary, Is.EqualTo("Products in catalog: 2"));

        var catalog = main.OpenCatalog();
        catalog.AddProduct("Notebook", "6.25").WaitUntilProductCount(3);
        catalog.Close();

        Retry.WhileFalse(
            () => main.ProductSummary == "Products in catalog: 3",
            TimeSpan.FromSeconds(3));

        Assert.That(main.ProductSummary, Is.EqualTo("Products in catalog: 3"));
    }
}
