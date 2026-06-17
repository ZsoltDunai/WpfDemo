using FlaUI.Core.Tools;
using NUnit.Framework;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MultiWindowE2ETests : E2ETestBase
{
    [Test]
    public void Saving_settings_updates_greeting_on_main_window()
    {
        Main.OpenSettings()
            .SetGreetingPrefix("Welcome")
            .Save()
            .SetAdminName("FlaUI")
            .SayHello();

        Retry.WhileFalse(
            () => Main.Greeting == "Welcome, FlaUI!",
            TimeSpan.FromSeconds(3));

        Assert.That(Main.Greeting, Is.EqualTo("Welcome, FlaUI!"));
    }

    [Test]
    public void Cancelling_settings_keeps_default_greeting_prefix()
    {
        Main.OpenSettings()
            .SetGreetingPrefix("Changed")
            .Cancel()
            .SetAdminName("Tester")
            .SayHello();

        Retry.WhileFalse(
            () => Main.Greeting == "Hello, Tester!",
            TimeSpan.FromSeconds(3));

        Assert.That(Main.Greeting, Is.EqualTo("Hello, Tester!"));
    }

    [Test]
    public void About_window_can_be_opened_and_closed_while_main_window_stays_open()
    {
        var about = Main.OpenAbout();

        Assert.That(about.AboutText, Does.Contain("Version 1.0.0"));

        about.CloseAbout();

        Assert.That(Session.FindWindowByTitle("About"), Is.Null);
        Assert.That(Session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);
        Assert.That(Main.Title, Is.EqualTo("Mini Shop Admin"));
    }

    [Test]
    public void Settings_and_about_windows_can_be_open_at_the_same_time()
    {
        var settings = Main.OpenSettings();
        var about = Main.OpenAbout();

        Assert.That(settings.Title, Is.EqualTo("Settings"));
        Assert.That(about.Title, Is.EqualTo("About"));
        Assert.That(about.AboutText, Does.Contain("end-to-end UI testing"));

        about.CloseAbout();

        Assert.That(Session.FindWindowByTitle("Settings"), Is.Not.Null);
        Assert.That(Session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);

        settings.Cancel();

        Assert.That(Session.FindWindowByTitle("Settings"), Is.Null);
        Assert.That(Session.FindWindowByTitle("About"), Is.Null);
        Assert.That(Session.FindWindowByTitle("Mini Shop Admin"), Is.Not.Null);
    }
}
