namespace WpfDemo.App.Services;

public interface IWindowFactory
{
    CatalogWindow CreateCatalogWindow();

    SettingsWindow CreateSettingsWindow();

    AboutWindow CreateAboutWindow();
}
