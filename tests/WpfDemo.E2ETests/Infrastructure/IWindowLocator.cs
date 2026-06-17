using FlaUI.Core.AutomationElements;

namespace WpfDemo.E2ETests.Infrastructure;

public interface IWindowLocator
{
    Window WaitForWindow(string title, TimeSpan? timeout = null);

    Window? FindWindowByTitle(string title);

    Window? FindWindowByAutomationId(string automationId);
}
