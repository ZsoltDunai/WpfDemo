using FlaUI.Core.AutomationElements;
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
