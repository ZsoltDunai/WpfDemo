using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using WpfDemo.E2ETests.TestData;
using WpfDemo.E2ETests.WindowObjects;

namespace WpfDemo.E2ETests.Infrastructure;

public sealed class WpfDemoAppSession : IUiSession, IDisposable
{
    private readonly UIA3Automation _automation = new();
    private readonly IAppExecutablePathResolver _pathResolver;
    private Application? _application;
    private FlaUiWindowLocator? _windowLocator;

    public WpfDemoAppSession(IAppExecutablePathResolver? pathResolver = null)
    {
        _pathResolver = pathResolver ?? new AppExecutablePathResolver();
    }

    public UIA3Automation Automation => _automation;

    public Application Application => _application
        ?? throw new InvalidOperationException("Launch the application before accessing it.");

    private FlaUiWindowLocator WindowLocator => _windowLocator
        ?? throw new InvalidOperationException("Launch the application before locating windows.");

    public static WpfDemoAppSession Launch()
    {
        var session = new WpfDemoAppSession();
        session.Start();
        return session;
    }

    public MainWindowObject OpenMainWindow()
    {
        var window = Application.GetMainWindow(_automation, UiTimeouts.Window);
        Assert.That(window, Is.Not.Null);

        return new MainWindowObject(window!, this);
    }

    public Window WaitForWindow(string title, TimeSpan? timeout = null)
    {
        return WindowLocator.WaitForWindow(title, timeout);
    }

    public Window? FindWindowByTitle(string title)
    {
        return WindowLocator.FindWindowByTitle(title);
    }

    public Window? FindWindowByAutomationId(string automationId)
    {
        return WindowLocator.FindWindowByAutomationId(automationId);
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
        _application = Application.Launch(_pathResolver.Resolve());
        _windowLocator = new FlaUiWindowLocator(_application, _automation);
    }
}
