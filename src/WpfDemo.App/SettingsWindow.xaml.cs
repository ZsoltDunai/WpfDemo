using System.Windows;
using System.Windows.Automation;
using WpfDemo.App.Services.Settings;
using WpfDemo.App.Ui;

namespace WpfDemo.App;

public partial class SettingsWindow : Window
{
    private readonly IAppSettingsService _appSettings;

    public SettingsWindow(IAppSettingsService appSettings)
    {
        _appSettings = appSettings;

        InitializeComponent();
        AutomationProperties.SetAutomationId(this, AutomationIds.SettingsWindow);
        GreetingPrefixTextBox.Text = _appSettings.GreetingPrefix;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _appSettings.GreetingPrefix = ReadGreetingPrefix();
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private string ReadGreetingPrefix()
    {
        var prefix = GreetingPrefixTextBox.Text.Trim();
        return string.IsNullOrWhiteSpace(prefix)
            ? AppMessages.DefaultGreetingPrefix
            : prefix;
    }
}
