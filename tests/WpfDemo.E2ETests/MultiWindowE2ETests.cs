using NUnit.Framework;
using WpfDemo.App.Ui;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MultiWindowE2ETests : E2ETestBase
{
    [Test]
    public void Saving_settings_updates_greeting_on_main_window()
    {
        const string expectedGreeting = "Welcome, FlaUI!";

        Main.UpdateGreetingPrefix("Welcome").GreetAs("FlaUI", expectedGreeting);

        Assert.That(Main.Greeting, Is.EqualTo(expectedGreeting));
    }

    [Test]
    public void Cancelling_settings_keeps_default_greeting_prefix()
    {
        const string expectedGreeting = "Hello, Tester!";

        Main.UpdateGreetingPrefix("Changed", save: false).GreetAs("Tester", expectedGreeting);

        Assert.That(Main.Greeting, Is.EqualTo(expectedGreeting));
    }

    [Test]
    public void About_window_can_be_opened_and_closed_while_main_window_stays_open()
    {
        var about = Main.OpenAbout();

        Assert.That(about.AboutText, Does.Contain("Version 1.0.0"));

        about.CloseAbout();

        AssertWindowClosed(WindowTitles.About);
        AssertWindowOpen(WindowTitles.Main);
        Assert.That(Main.Title, Is.EqualTo(WindowTitles.Main));
    }

    [Test]
    public void Settings_and_about_windows_can_be_open_at_the_same_time()
    {
        var settings = Main.OpenSettings();
        var about = Main.OpenAbout();

        Assert.That(settings.Title, Is.EqualTo(WindowTitles.Settings));
        Assert.That(about.Title, Is.EqualTo(WindowTitles.About));
        Assert.That(about.AboutText, Does.Contain("end-to-end UI testing"));

        about.CloseAbout();

        AssertWindowOpen(WindowTitles.Settings);
        AssertWindowOpen(WindowTitles.Main);

        settings.Cancel();

        AssertWindowClosed(WindowTitles.Settings);
        AssertWindowClosed(WindowTitles.About);
        AssertWindowOpen(WindowTitles.Main);
    }
}
