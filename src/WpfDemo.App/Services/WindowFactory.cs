using Microsoft.Extensions.DependencyInjection;

namespace WpfDemo.App.Services;

public sealed class WindowFactory : IWindowFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WindowFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public CatalogWindow CreateCatalogWindow() => _serviceProvider.GetRequiredService<CatalogWindow>();

    public SettingsWindow CreateSettingsWindow() => _serviceProvider.GetRequiredService<SettingsWindow>();

    public AboutWindow CreateAboutWindow() => _serviceProvider.GetRequiredService<AboutWindow>();
}
