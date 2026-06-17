using System.Windows;

namespace WpfDemo.App.Presentation;

public sealed class ChildWindowManager : IChildWindowManager
{
    public void ShowOrActivate<TWindow>(
        Window owner,
        TWindow? openWindow,
        Action<TWindow?> assignWindow,
        Func<TWindow> createWindow,
        Action? onClosed = null)
        where TWindow : Window
    {
        if (openWindow is { IsVisible: true })
        {
            openWindow.Activate();
            return;
        }

        var window = createWindow();
        window.Owner = owner;
        window.Closed += (_, _) =>
        {
            assignWindow(null);
            onClosed?.Invoke();
        };

        assignWindow(window);
        window.Show();
    }
}
