using System.Windows;
using System.Windows.Automation;
using WpfDemo.App.Services;

namespace WpfDemo.App;

public partial class SettingsWindow : Window
{
    private readonly IAppSettingsService _appSettings;

    public SettingsWindow(IAppSettingsService appSettings)
    {
        _appSettings = appSettings;

        InitializeComponent();
        AutomationProperties.SetAutomationId(this, "SettingsWindow");
        GreetingPrefixTextBox.Text = _appSettings.GreetingPrefix;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var prefix = GreetingPrefixTextBox.Text.Trim();
        _appSettings.GreetingPrefix = string.IsNullOrWhiteSpace(prefix) ? "Hello" : prefix;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
