using FlaUI.Core.Tools;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[Collection("WpfDemo")]
public class MultiWindowE2ETests
{
    [Fact]
    public void Saving_settings_updates_greeting_on_main_window()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenSettingsButton");
        var settingsWindow = fixture.WaitForWindow("Settings");
        settingsWindow.Focus();

        var prefixTextBox = WindowTestHelpers.RequireTextBox(settingsWindow, "GreetingPrefixTextBox");
        prefixTextBox.Focus();
        prefixTextBox.Text = "Welcome";

        WindowTestHelpers.ClickButton(settingsWindow, "SaveSettingsButton");

        Retry.WhileTrue(
            () => fixture.FindWindowByTitle("Settings") is not null,
            TimeSpan.FromSeconds(3));

        Assert.Null(fixture.FindWindowByTitle("Settings"));
        Assert.NotNull(fixture.FindWindowByTitle("Mini Shop Admin"));

        var nameTextBox = WindowTestHelpers.RequireTextBox(mainWindow, "NameTextBox");
        nameTextBox.Focus();
        nameTextBox.Text = "FlaUI";
        WindowTestHelpers.RequireButton(mainWindow, "GreetButton").Invoke();

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(mainWindow, "GreetingTextBox") == "Welcome, FlaUI!",
            TimeSpan.FromSeconds(3));

        Assert.Equal("Welcome, FlaUI!", WindowTestHelpers.GetTextBoxValue(mainWindow, "GreetingTextBox"));
    }

    [Fact]
    public void Cancelling_settings_keeps_default_greeting_prefix()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenSettingsButton");
        var settingsWindow = fixture.WaitForWindow("Settings");
        settingsWindow.Focus();

        var prefixTextBox = WindowTestHelpers.RequireTextBox(settingsWindow, "GreetingPrefixTextBox");
        prefixTextBox.Focus();
        prefixTextBox.Text = "Changed";

        WindowTestHelpers.ClickButton(settingsWindow, "CancelSettingsButton");

        Retry.WhileTrue(
            () => fixture.FindWindowByTitle("Settings") is not null,
            TimeSpan.FromSeconds(3));

        var nameTextBox = WindowTestHelpers.RequireTextBox(mainWindow, "NameTextBox");
        nameTextBox.Focus();
        nameTextBox.Text = "Tester";
        WindowTestHelpers.RequireButton(mainWindow, "GreetButton").Invoke();

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(mainWindow, "GreetingTextBox") == "Hello, Tester!",
            TimeSpan.FromSeconds(3));

        Assert.Equal("Hello, Tester!", WindowTestHelpers.GetTextBoxValue(mainWindow, "GreetingTextBox"));
    }

    [Fact]
    public void About_window_can_be_opened_and_closed_while_main_window_stays_open()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();

        WindowTestHelpers.ClickButton(mainWindow, "OpenAboutButton");
        var aboutWindow = fixture.WaitForWindow("About");
        aboutWindow.Focus();

        Assert.Contains("Version 1.0.0", WindowTestHelpers.GetTextBoxValue(aboutWindow, "AboutTextBox"));

        WindowTestHelpers.ClickButton(aboutWindow, "CloseAboutButton");

        Retry.WhileTrue(
            () => fixture.FindWindowByTitle("About") is not null,
            TimeSpan.FromSeconds(3));

        Assert.Null(fixture.FindWindowByTitle("About"));
        Assert.NotNull(fixture.FindWindowByTitle("Mini Shop Admin"));
        Assert.Equal("Mini Shop Admin", mainWindow.Title);
    }

    [Fact]
    public void Settings_and_about_windows_can_be_open_at_the_same_time()
    {
        using var fixture = new WpfDemoAppFixture();
        var mainWindow = fixture.LaunchMainWindow();
        mainWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenSettingsButton");
        var settingsWindow = fixture.WaitForWindow("Settings");
        settingsWindow.Focus();

        WindowTestHelpers.ClickButton(mainWindow, "OpenAboutButton");
        var aboutWindow = fixture.WaitForWindow("About");
        aboutWindow.Focus();

        Assert.Equal("Settings", settingsWindow.Title);
        Assert.Equal("About", aboutWindow.Title);
        Assert.Contains("end-to-end UI testing", WindowTestHelpers.GetTextBoxValue(aboutWindow, "AboutTextBox"));

        WindowTestHelpers.ClickButton(aboutWindow, "CloseAboutButton");
        Retry.WhileTrue(
            () => fixture.FindWindowByTitle("About") is not null,
            TimeSpan.FromSeconds(3));

        Assert.NotNull(fixture.FindWindowByTitle("Settings"));
        Assert.NotNull(fixture.FindWindowByTitle("Mini Shop Admin"));

        WindowTestHelpers.ClickButton(settingsWindow, "CancelSettingsButton");
        Retry.WhileTrue(
            () => fixture.FindWindowByTitle("Settings") is not null,
            TimeSpan.FromSeconds(3));

        Assert.Null(fixture.FindWindowByTitle("Settings"));
        Assert.Null(fixture.FindWindowByTitle("About"));
        Assert.NotNull(fixture.FindWindowByTitle("Mini Shop Admin"));
    }
}
