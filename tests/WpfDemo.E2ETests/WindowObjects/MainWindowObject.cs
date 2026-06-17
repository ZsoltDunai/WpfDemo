using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.Infrastructure;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class MainWindowObject : WindowObjectBase
{
    public MainWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string Greeting => GetTextBoxValue(AutomationIds.GreetingTextBox);

    public string ProductSummary => GetTextBoxValue(AutomationIds.ProductSummaryTextBox);

    public MainWindowObject SetAdminName(string name)
    {
        SetTextBoxValue(AutomationIds.NameTextBox, name);
        return this;
    }

    public MainWindowObject SayHello()
    {
        ClickButton(AutomationIds.GreetButton);
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
        WaitUntil(() => Greeting == expectedGreeting, timeout);
        return this;
    }

    public MainWindowObject WaitUntilProductSummary(string expectedSummary, TimeSpan? timeout = null)
    {
        WaitUntil(() => ProductSummary == expectedSummary, timeout);
        return this;
    }

    public CatalogWindowObject OpenCatalog()
    {
        Focus();
        ClickButton(AutomationIds.OpenCatalogButton);
        return new CatalogWindowObject(Session.WaitForWindow(WindowTitles.Catalog), Session);
    }

    public SettingsWindowObject OpenSettings()
    {
        Focus();
        ClickButton(AutomationIds.OpenSettingsButton);
        return new SettingsWindowObject(Session.WaitForWindow(WindowTitles.Settings), Session);
    }

    public AboutWindowObject OpenAbout()
    {
        Focus();
        ClickButton(AutomationIds.OpenAboutButton);
        return new AboutWindowObject(Session.WaitForWindow(WindowTitles.About), Session);
    }

    private static void WaitUntil(Func<bool> condition, TimeSpan? timeout)
    {
        Retry.WhileFalse(condition, timeout ?? UiTimeouts.Default);
    }
}
