using System.Windows;
using System.Windows.Automation;

namespace WpfDemo.App;

public partial class MainWindow : Window
{
    private SettingsWindow? _settingsWindow;
    private AboutWindow? _aboutWindow;
    private CatalogWindow? _catalogWindow;

    public MainWindow()
    {
        InitializeComponent();
        AutomationProperties.SetAutomationId(this, "MainWindow");
        RefreshProductSummary();
    }

    private void GreetButton_Click(object sender, RoutedEventArgs e)
    {
        var name = NameTextBox.Text.Trim();

        GreetingTextBox.Text = string.IsNullOrWhiteSpace(name)
            ? "Please enter a name first."
            : $"{AppSettings.GreetingPrefix}, {name}!";
    }

    private void OpenCatalogButton_Click(object sender, RoutedEventArgs e)
    {
        if (_catalogWindow is { IsVisible: true })
        {
            _catalogWindow.Activate();
            return;
        }

        _catalogWindow = new CatalogWindow
        {
            Owner = this,
        };
        _catalogWindow.Closed += (_, _) =>
        {
            _catalogWindow = null;
            RefreshProductSummary();
        };
        _catalogWindow.Show();
    }

    private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        if (_settingsWindow is { IsVisible: true })
        {
            _settingsWindow.Activate();
            return;
        }

        _settingsWindow = new SettingsWindow
        {
            Owner = this,
        };
        _settingsWindow.Closed += (_, _) => _settingsWindow = null;
        _settingsWindow.Show();
    }

    private void OpenAboutButton_Click(object sender, RoutedEventArgs e)
    {
        if (_aboutWindow is { IsVisible: true })
        {
            _aboutWindow.Activate();
            return;
        }

        _aboutWindow = new AboutWindow
        {
            Owner = this,
        };
        _aboutWindow.Closed += (_, _) => _aboutWindow = null;
        _aboutWindow.Show();
    }

    private void RefreshProductSummary()
    {
        ProductSummaryTextBox.Text = $"Products in catalog: {ProductCatalog.Count}";
    }
}
