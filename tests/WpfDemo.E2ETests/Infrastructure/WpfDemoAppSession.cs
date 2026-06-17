using System.IO;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;
using WpfDemo.E2ETests.WindowObjects;

namespace WpfDemo.E2ETests.Infrastructure;

public sealed class WpfDemoAppSession : IUiSession, IDisposable
{
    private static readonly Dictionary<string, string> WindowAutomationIds = new(StringComparer.Ordinal)
    {
        ["Mini Shop Admin"] = "MainWindow",
        ["Product Catalog"] = "CatalogWindow",
        ["Settings"] = "SettingsWindow",
        ["About"] = "AboutWindow",
    };

    private readonly UIA3Automation _automation = new();
    private Application? _application;

    public UIA3Automation Automation => _automation;

    public Application Application => _application
        ?? throw new InvalidOperationException("Launch the application before accessing it.");

    public static WpfDemoAppSession Launch()
    {
        var session = new WpfDemoAppSession();
        session.Start();
        return session;
    }

    public MainWindowObject OpenMainWindow()
    {
        var window = Application.GetMainWindow(_automation, TimeSpan.FromSeconds(10));
        Assert.That(window, Is.Not.Null);

        return new MainWindowObject(window!, this);
    }

    public Window WaitForWindow(string title, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(10);

        var result = Retry.WhileNull(
            () => FindWindowByTitle(title) ?? FindWindowByAutomationId(GetAutomationIdForTitle(title)),
            timeout: timeout.Value,
            ignoreException: true);

        Assert.That(result.Result, Is.Not.Null);
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

    public void Dispose()
    {
        try
        {
            _application?.Close();
        }
        catch
        {
            _application?.Kill();
        }

        _automation.Dispose();
    }

    private void Start()
    {
        var appPath = GetAppExecutablePath();
        _application = Application.Launch(appPath);
    }

    private static string GetAutomationIdForTitle(string title)
    {
        return WindowAutomationIds.TryGetValue(title, out var automationId) ? automationId : title;
    }

    private IEnumerable<Window> GetOpenWindows()
    {
        var processId = Application.ProcessId;
        var windows = new List<Window>();

        try
        {
            windows.AddRange(Application.GetAllTopLevelWindows(_automation));
        }
        catch
        {
            // Continue with desktop search.
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
            if (windows.Count == 0)
            {
                var mainWindow = Application.GetMainWindow(_automation, TimeSpan.FromMilliseconds(500));
                if (mainWindow is not null)
                {
                    windows.Add(mainWindow);
                }
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

    private static string GetAppExecutablePath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var appProjectName = "WpfDemo.App";

        var candidates = new[]
        {
            Path.Combine(baseDirectory, $"{appProjectName}.exe"),
            Path.Combine(baseDirectory, "..", "..", "..", "..", "src", appProjectName, "bin", "Debug", "net10.0-windows", $"{appProjectName}.exe"),
            Path.Combine(baseDirectory, "..", "..", "..", "..", "src", appProjectName, "bin", "Release", "net10.0-windows", $"{appProjectName}.exe"),
        };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        throw new FileNotFoundException(
            $"Could not find {appProjectName}.exe. Build the solution before running E2E tests.");
    }
}
