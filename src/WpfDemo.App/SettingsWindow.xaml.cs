using System.Windows;
using System.Windows.Automation;

namespace WpfDemo.App;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        AutomationProperties.SetAutomationId(this, "SettingsWindow");
        GreetingPrefixTextBox.Text = AppSettings.GreetingPrefix;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var prefix = GreetingPrefixTextBox.Text.Trim();
        AppSettings.GreetingPrefix = string.IsNullOrWhiteSpace(prefix) ? "Hello" : prefix;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
