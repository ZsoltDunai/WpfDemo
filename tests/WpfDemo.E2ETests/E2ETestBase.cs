using NUnit.Framework;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.Infrastructure;
using WpfDemo.E2ETests.WindowObjects;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public abstract class E2ETestBase
{
    protected WpfDemoAppSession Session { get; private set; } = null!;

    protected MainWindowObject Main { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Session = WpfDemoAppSession.Launch();
        Main = Session.OpenMainWindow();
    }

    [TearDown]
    public void TearDown()
    {
        Session.Dispose();
    }

    protected void AssertWindowClosed(string title)
    {
        Assert.That(Session.FindWindowByTitle(title), Is.Null, $"Expected window to be closed: {title}");
    }

    protected void AssertWindowOpen(string title)
    {
        Assert.That(Session.FindWindowByTitle(title), Is.Not.Null, $"Expected window to be open: {title}");
    }

    protected static string FormatProductSummary(int count)
    {
        return string.Format(AppMessages.ProductSummaryFormat, count);
    }
}
