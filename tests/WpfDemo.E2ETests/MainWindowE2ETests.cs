using NUnit.Framework;

namespace WpfDemo.E2ETests;

[TestFixture]
[NonParallelizable]
public class MainWindowE2ETests : E2ETestBase
{
    [Test]
    public void Window_opens_with_title_and_default_greeting()
    {
        Assert.That(Main.Title, Is.EqualTo("Mini Shop Admin"));
        Assert.That(Main.Greeting, Does.Contain("Enter a name"));
    }

    [Test]
    public void Clicking_say_hello_without_name_shows_validation_message()
    {
        Main.SayHelloExpecting("Please enter a name first.");

        Assert.That(Main.Greeting, Does.Contain("Please enter a name"));
    }

    [Test]
    public void Entering_name_and_clicking_say_hello_updates_greeting()
    {
        Main.GreetAs("FlaUI", "Hello, FlaUI!");

        Assert.That(Main.Greeting, Is.EqualTo("Hello, FlaUI!"));
    }
}
