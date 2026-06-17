using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace WpfDemo.E2ETests.Infrastructure;

internal static class WindowTestHelpers
{
    public static string GetTextBoxValue(AutomationElement window, string automationId)
    {
        var textBox = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsTextBox();
        Assert.NotNull(textBox);

        return textBox.Text;
    }

    public static TextBox RequireTextBox(AutomationElement window, string automationId)
    {
        var textBox = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsTextBox();
        Assert.NotNull(textBox);

        return textBox;
    }

    public static Button RequireButton(AutomationElement window, string automationId)
    {
        var button = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsButton();
        Assert.NotNull(button);

        return button;
    }

    public static ListBox RequireListBox(AutomationElement window, string automationId)
    {
        var listBox = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsListBox();
        Assert.NotNull(listBox);

        return listBox;
    }

    public static void ClickButton(AutomationElement window, string automationId)
    {
        RequireButton(window, automationId).Invoke();
    }

    public static IReadOnlyList<string> GetListItemTexts(AutomationElement window, string listAutomationId)
    {
        return RequireListBox(window, listAutomationId)
            .Items
            .Select(item => item.Text)
            .ToArray();
    }

    public static IReadOnlyList<string>? TryGetListItemTexts(AutomationElement window, string listAutomationId)
    {
        var listBox = window.FindFirstDescendant(cf => cf.ByAutomationId(listAutomationId))?.AsListBox();
        if (listBox is null)
        {
            return null;
        }

        return listBox.Items.Select(item => item.Text).ToArray();
    }

    public static ListBoxItem RequireListItem(AutomationElement window, string listAutomationId, string itemText)
    {
        var item = RequireListBox(window, listAutomationId)
            .Items
            .FirstOrDefault(candidate => candidate.Text == itemText);

        Assert.NotNull(item);
        return item;
    }

    public static void InvokeContextMenuItem(UIA3Automation automation, AutomationElement target, string menuItemAutomationId)
    {
        target.RightClick();

        var result = Retry.WhileNull(
            () => automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId(menuItemAutomationId))?.AsMenuItem(),
            timeout: TimeSpan.FromSeconds(3),
            ignoreException: true);

        Assert.NotNull(result.Result);
        result.Result.Invoke();

        Retry.WhileTrue(
            () => automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId(menuItemAutomationId)) is not null,
            timeout: TimeSpan.FromSeconds(2),
            ignoreException: true);
    }
}
