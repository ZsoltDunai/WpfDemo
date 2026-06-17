using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;
using WpfDemo.App.Ui;
using WpfDemo.E2ETests.TestData;

namespace WpfDemo.E2ETests.Infrastructure;

public sealed class FlaUiWindowLocator : IWindowLocator
{
    private static readonly Dictionary<string, string> WindowAutomationIds = new(StringComparer.Ordinal)
    {
        [WindowTitles.Main] = AutomationIds.MainWindow,
        [WindowTitles.Catalog] = AutomationIds.CatalogWindow,
        [WindowTitles.Settings] = AutomationIds.SettingsWindow,
        [WindowTitles.About] = AutomationIds.AboutWindow,
    };

    private readonly Application _application;
    private readonly UIA3Automation _automation;

    public FlaUiWindowLocator(Application application, UIA3Automation automation)
    {
        _application = application;
        _automation = automation;
    }

    public Window WaitForWindow(string title, TimeSpan? timeout = null)
    {
        var result = Retry.WhileNull(
            () => FindWindowByTitle(title) ?? FindWindowByAutomationId(GetAutomationIdForTitle(title)),
            timeout: timeout ?? UiTimeouts.Window,
            ignoreException: true);

        Assert.That(result.Result, Is.Not.Null, $"Window not found: {title}");
        return result.Result!;
    }

    public Window? FindWindowByTitle(string title)
    {
        return GetOpenWindows().FirstOrDefault(window => window.Title == title);
    }

    public Window? FindWindowByAutomationId(string automationId)
    {
        if (string.IsNullOrWhiteSpace(automationId))
        {
            return null;
        }

        var window = GetOpenWindows().FirstOrDefault(candidate => candidate.AutomationId == automationId);
        if (window is not null)
        {
            return window;
        }

        try
        {
            var desktop = _automation.GetDesktop();
            var element = desktop.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
            return element is null ? null : GetContainingWindow(element);
        }
        catch
        {
            return null;
        }
    }

    private static string GetAutomationIdForTitle(string title)
    {
        return WindowAutomationIds.TryGetValue(title, out var automationId) ? automationId : title;
    }

    private IEnumerable<Window> GetOpenWindows()
    {
        var processId = _application.ProcessId;
        var windows = new List<Window>();

        try
        {
            windows.AddRange(_application.GetAllTopLevelWindows(_automation));
        }
        catch
        {
            // Fall back to desktop search.
        }

        try
        {
            var desktop = _automation.GetDesktop();
            windows.AddRange(
                desktop
                    .FindAllDescendants(cf => cf.ByControlType(ControlType.Window))
                    .Where(window => window.Properties.ProcessId.Value == processId)
                    .Select(window => window.AsWindow()));
        }
        catch
        {
            var mainWindow = _application.GetMainWindow(_automation, TimeSpan.FromMilliseconds(500));
            if (mainWindow is not null)
            {
                windows.Add(mainWindow);
            }
        }

        return windows
            .GroupBy(window => window.Properties.NativeWindowHandle.Value)
            .Select(group => group.First());
    }

    private static Window? GetContainingWindow(AutomationElement element)
    {
        var current = element;
        while (current is not null)
        {
            if (current.ControlType == ControlType.Window)
            {
                return current.AsWindow();
            }

            current = current.Parent;
        }

        return null;
    }
}
