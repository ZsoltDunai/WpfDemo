using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class SettingsWindowObject : WindowObjectBase
{
    public SettingsWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public SettingsWindowObject SetGreetingPrefix(string prefix)
    {
        Focus();
        SetTextBoxValue("GreetingPrefixTextBox", prefix);
        return this;
    }

    public MainWindowObject Save()
    {
        ClickButton("SaveSettingsButton");
        WaitUntilClosed();
        return new MainWindowObject(Session.WaitForWindow("Mini Shop Admin"), Session);
    }

    public MainWindowObject Cancel()
    {
        ClickButton("CancelSettingsButton");
        WaitUntilClosed();
        return new MainWindowObject(Session.WaitForWindow("Mini Shop Admin"), Session);
    }

    private void WaitUntilClosed()
    {
        Retry.WhileTrue(
            () => Session.FindWindowByTitle("Settings") is not null,
            TimeSpan.FromSeconds(3));
    }
}
