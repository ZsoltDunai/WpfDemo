using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace WpfDemo.E2ETests.Infrastructure;

public interface IUiSession
{
    UIA3Automation Automation { get; }

    Window WaitForWindow(string title, TimeSpan? timeout = null);

    Window? FindWindowByTitle(string title);
}
