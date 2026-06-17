using NUnit.Framework;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MainWindowE2ETests : E2ETestBase
{
    [Test]
    public void Window_opens_with_title_and_default_greeting()
    {
        Assert.That(Main.Title, Is.EqualTo(WindowTitles.Main));
        Assert.That(Main.Greeting, Does.Contain("Enter a name"));
    }

    [Test]
    public void Clicking_say_hello_without_name_shows_validation_message()
    {
        Main.SayHelloExpecting(AppMessages.NameRequired);

        Assert.That(Main.Greeting, Does.Contain("Please enter a name"));
    }

    [Test]
    public void Entering_name_and_clicking_say_hello_updates_greeting()
    {
        const string expectedGreeting = "Hello, FlaUI!";

        Main.GreetAs("FlaUI", expectedGreeting);

        Assert.That(Main.Greeting, Is.EqualTo(expectedGreeting));
    }
}
