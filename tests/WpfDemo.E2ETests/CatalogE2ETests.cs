using NUnit.Framework;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class CatalogE2ETests : E2ETestBase
{
    [Test]
    public void Adding_product_updates_list_and_status()
    {
        var catalog = Main.OpenCatalog();

        Assert.That(catalog.Products, Is.EqualTo(TestProducts.DefaultCatalog));

        catalog.AddProduct("Tea", "3.50").WaitUntilProductAppears(TestProducts.Tea);

        Assert.That(catalog.Products, Does.Contain(TestProducts.Tea));
        Assert.That(catalog.Status, Is.EqualTo($"Added {TestProducts.Tea}."));
    }

    [Test]
    public void Adding_product_without_name_shows_validation()
    {
        var catalog = Main.OpenCatalog();

        catalog.AddProductWithoutName().WaitUntilStatus(AppMessages.ProductNameRequired);

        Assert.That(catalog.Products, Has.Count.EqualTo(TestProducts.DefaultCatalog.Length));
    }

    [Test]
    public void Closing_catalog_updates_product_summary_on_main_window()
    {
        Assert.That(Main.ProductSummary, Is.EqualTo(FormatProductSummary(2)));

        var catalog = Main.OpenCatalog();
        catalog.AddProduct("Notebook", "6.25").WaitUntilProductCount(3);
        catalog.Close();

        Main.WaitUntilProductSummary(FormatProductSummary(3));

        Assert.That(Main.ProductSummary, Is.EqualTo(FormatProductSummary(3)));
    }
}
