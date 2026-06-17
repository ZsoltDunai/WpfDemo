using FlaUI.UIA3;

namespace WpfDemo.E2ETests.Infrastructure;

public interface IAutomationProvider
{
    UIA3Automation Automation { get; }
}
