using NUnit.Framework;
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
}

