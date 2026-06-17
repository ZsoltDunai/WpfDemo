using NUnit.Framework;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MultiWindowE2ETests : E2ETestBase
{
    [Test]
    public void Saving_settings_updates_greeting_on_main_window()
    {
        Main.UpdateGreetingPrefix("Welcome").GreetAs("FlaUI", "Welcome, FlaUI!");

        Assert.That(Main.Greeting, Is.EqualTo("Welcome, FlaUI!"));
    }

    [Test]
    public void Cancelling_settings_keeps_default_greeting_prefix()
    {
        Main.UpdateGreetingPrefix("Changed", save: false).GreetAs("Tester", "Hello, Tester!");

        Assert.That(Main.Greeting, Is.EqualTo("Hello, Tester!"));
    }

    [Test]
    public void About_window_can_be_opened_and_closed_while_main_window_stays_open()
    {
        var about = Main.OpenAbout();

        Assert.That(about.AboutText, Does.Contain("Version 1.0.0"));

        about.CloseAbout();

        AssertWindowClosed("About");
        AssertWindowOpen("Mini Shop Admin");
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

        AssertWindowOpen("Settings");
        AssertWindowOpen("Mini Shop Admin");

        settings.Cancel();

        AssertWindowClosed("Settings");
        AssertWindowClosed("About");
        AssertWindowOpen("Mini Shop Admin");
    }
}
