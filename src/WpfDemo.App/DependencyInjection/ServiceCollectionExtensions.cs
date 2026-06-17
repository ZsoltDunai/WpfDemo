using Microsoft.Extensions.DependencyInjection;
using WpfDemo.App.Presentation;
using WpfDemo.App.Services.Catalog;
using WpfDemo.App.Services.Greeting;
using WpfDemo.App.Services.Settings;
using WpfDemo.App.Services.Windows;

namespace WpfDemo.App.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpfDemoApp(this IServiceCollection services)
    {
        services.AddSingleton<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<ProductCatalogService>();
        services.AddSingleton<IProductCatalogService>(static provider => provider.GetRequiredService<ProductCatalogService>());
        services.AddSingleton<IProductCatalogReader>(static provider => provider.GetRequiredService<IProductCatalogService>());
        services.AddSingleton<IProductInputValidator, ProductInputValidator>();
        services.AddSingleton<IGreetingService, GreetingService>();
        services.AddSingleton<IChildWindowManager, ChildWindowManager>();
        services.AddSingleton<IWindowFactory, WindowFactory>();

        services.AddTransient<MainWindow>();
        services.AddTransient<CatalogWindow>();
        services.AddTransient<SettingsWindow>();
        services.AddTransient<AboutWindow>();

        return services;
    }
}
