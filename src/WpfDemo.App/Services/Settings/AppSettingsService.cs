using WpfDemo.App.Ui;

namespace WpfDemo.App.Services.Settings;

public sealed class AppSettingsService : IAppSettingsService
{
    public string GreetingPrefix { get; set; } = AppMessages.DefaultGreetingPrefix;
}
