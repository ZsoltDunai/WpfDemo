using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.Infrastructure;
using WpfDemo.E2ETests.TestData;

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
        SetTextBoxValue(AutomationIds.GreetingPrefixTextBox, prefix);
        return this;
    }

    public MainWindowObject Save()
    {
        ClickButton(AutomationIds.SaveSettingsButton);
        return CloseAndReturnToMain();
    }

    public MainWindowObject Cancel()
    {
        ClickButton(AutomationIds.CancelSettingsButton);
        return CloseAndReturnToMain();
    }

    private MainWindowObject CloseAndReturnToMain()
    {
        WaitUntilSettingsClosed();
        return new MainWindowObject(Session.WaitForWindow(WindowTitles.Main), Session);
    }

    private void WaitUntilSettingsClosed()
    {
        Retry.WhileTrue(
            () => Session.FindWindowByTitle(WindowTitles.Settings) is not null,
            UiTimeouts.Default);
    }
}
