using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;
using WpfDemo.E2ETests.Infrastructure;

namespace WpfDemo.E2ETests.WindowObjects;

public abstract class WindowObjectBase
{
    protected WindowObjectBase(Window window, IUiSession session)
    {
        Window = window;
        Session = session;
    }

    protected Window Window { get; }

    protected IUiSession Session { get; }

    public string Title => Window.Title;

    public void Focus() => Window.Focus();

    public void Close() => Window.Close();

    protected string GetTextBoxValue(string automationId)
    {
        return RequireTextBox(automationId).Text;
    }

    protected void SetTextBoxValue(string automationId, string value)
    {
        var textBox = RequireTextBox(automationId);
        textBox.Focus();
        textBox.Text = value;
    }

    protected void ClickButton(string automationId)
    {
        RequireButton(automationId).Invoke();
    }

    protected TextBox RequireTextBox(string automationId)
    {
        var textBox = Window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsTextBox();
        Assert.That(textBox, Is.Not.Null);
        return textBox!;
    }

    protected Button RequireButton(string automationId)
    {
        var button = Window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsButton();
        Assert.That(button, Is.Not.Null);
        return button!;
    }

    protected ListBox RequireListBox(string automationId)
    {
        var listBox = Window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsListBox();
        Assert.That(listBox, Is.Not.Null);
        return listBox!;
    }

    protected IReadOnlyList<string> GetListItemTexts(string listAutomationId)
    {
        return RequireListBox(listAutomationId).Items.Select(item => item.Text).ToArray();
    }

    protected IReadOnlyList<string>? TryGetListItemTexts(string listAutomationId)
    {
        var listBox = Window.FindFirstDescendant(cf => cf.ByAutomationId(listAutomationId))?.AsListBox();
        if (listBox is null)
        {
            return null;
        }

        return listBox.Items.Select(item => item.Text).ToArray();
    }

    protected ListBoxItem RequireListItem(string listAutomationId, string itemText)
    {
        var item = RequireListBox(listAutomationId)
            .Items
            .FirstOrDefault(candidate => candidate.Text == itemText);

        Assert.That(item, Is.Not.Null);
        return item!;
    }

    protected void InvokeContextMenuItem(AutomationElement target, string menuItemAutomationId)
    {
        target.RightClick();

        var result = Retry.WhileNull(
            () => Session.Automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId(menuItemAutomationId))?.AsMenuItem(),
            timeout: TimeSpan.FromSeconds(3),
            ignoreException: true);

        Assert.That(result.Result, Is.Not.Null);
        result.Result!.Invoke();

        Retry.WhileTrue(
            () => Session.Automation.GetDesktop().FindFirstDescendant(cf => cf.ByAutomationId(menuItemAutomationId)) is not null,
            timeout: TimeSpan.FromSeconds(2),
            ignoreException: true);
    }
}
