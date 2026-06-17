using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MainWindowE2ETests
{
    [Test]
    public void Window_opens_with_title_and_default_greeting()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        Assert.That(main.Title, Is.EqualTo("Mini Shop Admin"));
        Assert.That(main.Greeting, Does.Contain("Enter a name"));
    }

    [Test]
    public void Clicking_say_hello_without_name_shows_validation_message()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        main.SayHello();

        Retry.WhileFalse(
            () => main.Greeting.Contains("Please enter a name", StringComparison.Ordinal),
            TimeSpan.FromSeconds(3));

        Assert.That(main.Greeting, Does.Contain("Please enter a name"));
    }

    [Test]
    public void Entering_name_and_clicking_say_hello_updates_greeting()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        main.SetAdminName("FlaUI").SayHello();

        Retry.WhileFalse(
            () => main.Greeting == "Hello, FlaUI!",
            TimeSpan.FromSeconds(3));

        Assert.That(main.Greeting, Is.EqualTo("Hello, FlaUI!"));
    }
}
