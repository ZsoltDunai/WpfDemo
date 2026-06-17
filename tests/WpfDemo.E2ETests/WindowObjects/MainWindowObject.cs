using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class MainWindowObject : WindowObjectBase
{
    public MainWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string Greeting => GetTextBoxValue("GreetingTextBox");

    public string ProductSummary => GetTextBoxValue("ProductSummaryTextBox");

    public MainWindowObject SetAdminName(string name)
    {
        SetTextBoxValue("NameTextBox", name);
        return this;
    }

    public MainWindowObject SayHello()
    {
        ClickButton("GreetButton");
        return this;
    }

    public MainWindowObject GreetAs(string name, string expectedGreeting)
    {
        return SetAdminName(name).SayHello().WaitUntilGreeting(expectedGreeting);
    }

    public MainWindowObject SayHelloExpecting(string expectedGreeting)
    {
        return SayHello().WaitUntilGreeting(expectedGreeting);
    }

    public MainWindowObject UpdateGreetingPrefix(string prefix, bool save = true)
    {
        var settings = OpenSettings().SetGreetingPrefix(prefix);
        return save ? settings.Save() : settings.Cancel();
    }

    public MainWindowObject WaitUntilGreeting(string expectedGreeting, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);

        Retry.WhileFalse(
            () => Greeting == expectedGreeting,
            timeout.Value);

        return this;
    }

    public MainWindowObject WaitUntilProductSummary(string expectedSummary, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);

        Retry.WhileFalse(
            () => ProductSummary == expectedSummary,
            timeout.Value);

        return this;
    }

    public CatalogWindowObject OpenCatalog()
    {
        Focus();
        ClickButton("OpenCatalogButton");
        return new CatalogWindowObject(Session.WaitForWindow("Product Catalog"), Session);
    }

    public SettingsWindowObject OpenSettings()
    {
        Focus();
        ClickButton("OpenSettingsButton");
        return new SettingsWindowObject(Session.WaitForWindow("Settings"), Session);
    }

    public AboutWindowObject OpenAbout()
    {
        Focus();
        ClickButton("OpenAboutButton");
        return new AboutWindowObject(Session.WaitForWindow("About"), Session);
    }
}
