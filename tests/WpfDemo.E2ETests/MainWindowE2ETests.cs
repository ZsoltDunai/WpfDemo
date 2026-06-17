using FlaUI.Core.Tools;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests;

[Collection("WpfDemo")]
public class MainWindowE2ETests
{
    [Fact]
    public void Window_opens_with_title_and_default_greeting()
    {
        using var fixture = new WpfDemoAppFixture();
        var window = fixture.LaunchMainWindow();

        Assert.Equal("Mini Shop Admin", window.Title);
        Assert.Contains("Enter a name", WindowTestHelpers.GetTextBoxValue(window, "GreetingTextBox"));
    }

    [Fact]
    public void Clicking_say_hello_without_name_shows_validation_message()
    {
        using var fixture = new WpfDemoAppFixture();
        var window = fixture.LaunchMainWindow();

        WindowTestHelpers.RequireButton(window, "GreetButton").Invoke();

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(window, "GreetingTextBox").Contains("Please enter a name", StringComparison.Ordinal),
            TimeSpan.FromSeconds(3));

        Assert.Contains("Please enter a name", WindowTestHelpers.GetTextBoxValue(window, "GreetingTextBox"));
    }

    [Fact]
    public void Entering_name_and_clicking_say_hello_updates_greeting()
    {
        using var fixture = new WpfDemoAppFixture();
        var window = fixture.LaunchMainWindow();

        var nameTextBox = WindowTestHelpers.RequireTextBox(window, "NameTextBox");
        var greetButton = WindowTestHelpers.RequireButton(window, "GreetButton");

        nameTextBox.Focus();
        nameTextBox.Text = "FlaUI";
        greetButton.Invoke();

        Retry.WhileFalse(
            () => WindowTestHelpers.GetTextBoxValue(window, "GreetingTextBox") == "Hello, FlaUI!",
            TimeSpan.FromSeconds(3));

        Assert.Equal("Hello, FlaUI!", WindowTestHelpers.GetTextBoxValue(window, "GreetingTextBox"));
    }
}
