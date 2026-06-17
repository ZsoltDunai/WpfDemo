using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.Infrastructure;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class AboutWindowObject : WindowObjectBase
{
    public AboutWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string AboutText => GetTextBoxValue(AutomationIds.AboutTextBox);

    public MainWindowObject CloseAbout()
    {
        Focus();
        ClickButton(AutomationIds.CloseAboutButton);
        WaitUntilAboutClosed();
        return new MainWindowObject(Session.WaitForWindow(WindowTitles.Main), Session);
    }

    private void WaitUntilAboutClosed()
    {
        Retry.WhileTrue(
            () => Session.FindWindowByTitle(WindowTitles.About) is not null,
            UiTimeouts.Default);
    }
}
