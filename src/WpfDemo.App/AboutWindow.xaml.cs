using System.Windows;
using System.Windows.Automation;
using WpfDemo.App.Ui;

namespace WpfDemo.App;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        AutomationProperties.SetAutomationId(this, AutomationIds.AboutWindow);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
