using System.Windows;
using System.Windows.Automation;
using WpfDemo.App.Presentation;
using WpfDemo.App.Services.Catalog;
using WpfDemo.App.Services.Greeting;
using WpfDemo.App.Services.Windows;
using WpfDemo.App.Ui;

namespace WpfDemo.App;

public partial class MainWindow : Window
{
    private readonly IGreetingService _greetingService;
    private readonly IProductCatalogReader _productCatalog;
    private readonly IWindowFactory _windowFactory;
    private readonly IChildWindowManager _childWindowManager;

    private SettingsWindow? _settingsWindow;
    private AboutWindow? _aboutWindow;
    private CatalogWindow? _catalogWindow;

    public MainWindow(
        IGreetingService greetingService,
        IProductCatalogReader productCatalog,
        IWindowFactory windowFactory,
        IChildWindowManager childWindowManager)
    {
        _greetingService = greetingService;
        _productCatalog = productCatalog;
        _windowFactory = windowFactory;
        _childWindowManager = childWindowManager;

        InitializeComponent();
        AutomationProperties.SetAutomationId(this, AutomationIds.MainWindow);
        RefreshProductSummary();
    }

    private void GreetButton_Click(object sender, RoutedEventArgs e)
    {
        GreetingTextBox.Text = _greetingService.BuildGreeting(NameTextBox.Text.Trim());
    }

    private void OpenCatalogButton_Click(object sender, RoutedEventArgs e)
    {
        _childWindowManager.ShowOrActivate(
            this,
            _catalogWindow,
            window => _catalogWindow = window,
            _windowFactory.CreateCatalogWindow,
            RefreshProductSummary);
    }

    private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        _childWindowManager.ShowOrActivate(
            this,
            _settingsWindow,
            window => _settingsWindow = window,
            _windowFactory.CreateSettingsWindow);
    }

    private void OpenAboutButton_Click(object sender, RoutedEventArgs e)
    {
        _childWindowManager.ShowOrActivate(
            this,
            _aboutWindow,
            window => _aboutWindow = window,
            _windowFactory.CreateAboutWindow);
    }

    private void RefreshProductSummary()
    {
        ProductSummaryTextBox.Text = string.Format(
            AppMessages.ProductSummaryFormat,
            _productCatalog.Count);
    }
}
