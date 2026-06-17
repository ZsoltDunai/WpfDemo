using System.Windows;
using System.Windows.Automation;

namespace WpfDemo.App;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        AutomationProperties.SetAutomationId(this, "AboutWindow");
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
