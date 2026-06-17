namespace WpfDemo.App.Services;

public sealed class AppSettingsService : IAppSettingsService
{
    public string GreetingPrefix { get; set; } = "Hello";
}
