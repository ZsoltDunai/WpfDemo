namespace WpfDemo.App.Services.Windows;

public interface IWindowFactory
{
    CatalogWindow CreateCatalogWindow();

    SettingsWindow CreateSettingsWindow();

    AboutWindow CreateAboutWindow();
}
