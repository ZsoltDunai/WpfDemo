using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests.WindowObjects;

public sealed class AboutWindowObject : WindowObjectBase
{
    public AboutWindowObject(Window window, IUiSession session)
        : base(window, session)
    {
    }

    public string AboutText => GetTextBoxValue("AboutTextBox");

    public MainWindowObject CloseAbout()
    {
        Focus();
        ClickButton("CloseAboutButton");
        WaitUntilClosed();
        return new MainWindowObject(Session.WaitForWindow("Mini Shop Admin"), Session);
    }

    private void WaitUntilClosed()
    {
        Retry.WhileTrue(
            () => Session.FindWindowByTitle("About") is not null,
            TimeSpan.FromSeconds(3));
    }
}
