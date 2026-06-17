using Microsoft.Extensions.DependencyInjection;
using WpfDemo.App.Services;

namespace WpfDemo.App.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpfDemoApp(this IServiceCollection services)
    {
        services.AddSingleton<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<IProductCatalogService, ProductCatalogService>();
        services.AddSingleton<IWindowFactory, WindowFactory>();

        services.AddTransient<MainWindow>();
        services.AddTransient<CatalogWindow>();
        services.AddTransient<SettingsWindow>();
        services.AddTransient<AboutWindow>();

        return services;
    }
}
