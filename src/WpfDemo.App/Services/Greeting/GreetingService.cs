using WpfDemo.App.Services.Settings;
using WpfDemo.App.Ui;

namespace WpfDemo.App.Services.Greeting;

public sealed class GreetingService : IGreetingService
{
    private readonly IAppSettingsService _appSettings;

    public GreetingService(IAppSettingsService appSettings)
    {
        _appSettings = appSettings;
    }

    public string BuildGreeting(string adminName)
    {
        return string.IsNullOrWhiteSpace(adminName)
            ? AppMessages.NameRequired
            : $"{_appSettings.GreetingPrefix}, {adminName}!";
    }
}
