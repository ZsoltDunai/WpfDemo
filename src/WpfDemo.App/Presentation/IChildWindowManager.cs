using System.Windows;

namespace WpfDemo.App.Presentation;

public interface IChildWindowManager
{
    void ShowOrActivate<TWindow>(
        Window owner,
        TWindow? openWindow,
        Action<TWindow?> assignWindow,
        Func<TWindow> createWindow,
        Action? onClosed = null)
        where TWindow : Window;
}
