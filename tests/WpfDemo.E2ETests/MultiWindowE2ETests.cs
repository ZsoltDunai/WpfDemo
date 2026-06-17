using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MultiWindowE2ETests
{
    [Test]
    public void Saving_settings_updates_greeting_on_main_window()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        main.OpenSettings()
            .SetGreetingPrefix("Welcome")
            .Save()
            .SetAdminName("FlaUI")
            .SayHello();

        Retry.WhileFalse(
            () => main.Greeting == "Welcome, FlaUI!",
            TimeSpan.FromSeconds(3));

        Assert.That(main.Greeting, Is.EqualTo("Welcome, FlaUI!"));
    }

    [Test]
    public void Cancelling_settings_keeps_default_greeting_prefix()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        main.OpenSettings()
            .SetGreetingPrefix("Changed")
            .Cancel()
            .SetAdminName("Tester")
            .SayHello();

        Retry.WhileFalse(
            () => main.Greeting == "Hello, Tester!",
            TimeSpan.FromSeconds(3));

        Assert.That(main.Greeting, Is.EqualTo("Hello, Tester!"));
    }

    [Test]
    public void About_window_can_be_opened_and_closed_while_main_window_stays_open()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        var about = main.OpenAbout();

        Assert.That(about.AboutText, Does.Contain("Version 1.0.0"));

        about.CloseAbout();

        Assert.That(session.FindWindowByTitle("About"), Is.Null);
        Assert.That(session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);
        Assert.That(main.Title, Is.EqualTo("Mini Shop Admin"));
    }

    [Test]
    public void Settings_and_about_windows_can_be_open_at_the_same_time()
    {
        using var session = WpfDemoAppSession.Launch();
        var main = session.OpenMainWindow();

        var settings = main.OpenSettings();
        var about = main.OpenAbout();

        Assert.That(settings.Title, Is.EqualTo("Settings"));
        Assert.That(about.Title, Is.EqualTo("About"));
        Assert.That(about.AboutText, Does.Contain("end-to-end UI testing"));

        about.CloseAbout();

        Assert.That(session.FindWindowByTitle("Settings"), Is.Not.Null);
        Assert.That(session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);

        settings.Cancel();

        Assert.That(session.FindWindowByTitle("Settings"), Is.Null);
        Assert.That(session.FindWindowByTitle("About"), Is.Null);
        Assert.That(session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);
    }
}
