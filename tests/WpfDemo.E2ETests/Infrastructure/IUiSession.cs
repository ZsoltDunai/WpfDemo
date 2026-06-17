using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace WpfDemo.E2ETests.Infrastructure;

public interface IUiSession : IAutomationProvider, IWindowLocator
{
}
