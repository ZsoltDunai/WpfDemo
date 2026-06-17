using NUnit.Framework;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class CatalogE2ETests : E2ETestBase
{
    [Test]
    public void Adding_product_updates_list_and_status()
    {
        var catalog = Main.OpenCatalog();

        Assert.That(catalog.Products, Is.EqualTo(new[] { "Coffee - $4.50", "Mug - $12.00" }));

        catalog.AddProduct("Tea", "3.50").WaitUntilProductAppears("Tea - $3.50");

        Assert.That(catalog.Products, Does.Contain("Tea - $3.50"));
        Assert.That(catalog.Status, Is.EqualTo("Added Tea - $3.50."));
    }

    [Test]
    public void Adding_product_without_name_shows_validation()
    {
        var catalog = Main.OpenCatalog();

        catalog.AddProductWithoutName().WaitUntilStatus("Enter a product name.");

        Assert.That(catalog.Products, Has.Count.EqualTo(2));
    }

    [Test]
    public void Closing_catalog_updates_product_summary_on_main_window()
    {
        Assert.That(Main.ProductSummary, Is.EqualTo("Products in catalog: 2"));

        var catalog = Main.OpenCatalog();
        catalog.AddProduct("Notebook", "6.25").WaitUntilProductCount(3);
        catalog.Close();

        Main.WaitUntilProductSummary("Products in catalog: 3");

        Assert.That(Main.ProductSummary, Is.EqualTo("Products in catalog: 3"));
    }
}
