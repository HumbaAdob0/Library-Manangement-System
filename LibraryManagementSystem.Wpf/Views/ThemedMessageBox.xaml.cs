using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem.Views;

public partial class ThemedMessageBox : Window
{
    private MessageBoxResult _result = MessageBoxResult.None;

    private ThemedMessageBox(string message, string caption, MessageBoxButton buttons, MessageBoxImage image)
    {
        InitializeComponent();

        Title = caption;
        TitleTextBlock.Text = string.IsNullOrWhiteSpace(caption) ? "Message" : caption;
        MessageTextBlock.Text = message;
        ConfigureIcon(image);
        ConfigureButtons(buttons);
    }

    public static MessageBoxResult Show(string messageBoxText)
    {
        return Show(messageBoxText, "Message", MessageBoxButton.OK, MessageBoxImage.None);
    }

    public static MessageBoxResult Show(string messageBoxText, string caption)
    {
        return Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None);
    }

    public static MessageBoxResult Show(
        string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon)
    {
        var dialog = new ThemedMessageBox(messageBoxText, caption, button, icon);
        var owner = Application.Current?.Windows
            .OfType<Window>()
            .FirstOrDefault(window => window.IsActive && window != dialog)
            ?? Application.Current?.MainWindow;

        if (owner != null && owner != dialog)
        {
            dialog.Owner = owner;
        }

        dialog.ShowDialog();
        return dialog._result;
    }

    private void ConfigureIcon(MessageBoxImage image)
    {
        var iconText = image switch
        {
            MessageBoxImage.Warning => "!",
            MessageBoxImage.Error => "x",
            MessageBoxImage.Question => "?",
            MessageBoxImage.Information => "i",
            _ => "i"
        };

        IconTextBlock.Text = iconText;
        IconBorder.Background = image switch
        {
            MessageBoxImage.Error => new SolidColorBrush(Color.FromRgb(232, 213, 205)),
            MessageBoxImage.Warning => new SolidColorBrush(Color.FromRgb(238, 229, 216)),
            MessageBoxImage.Question => FindResource("AccentSage") as Brush,
            _ => FindResource("AccentClay") as Brush
        };
    }

    private void ConfigureButtons(MessageBoxButton buttons)
    {
        ButtonsPanel.Children.Clear();

        switch (buttons)
        {
            case MessageBoxButton.OKCancel:
                AddButton("Cancel", MessageBoxResult.Cancel, false);
                AddButton("OK", MessageBoxResult.OK, true);
                break;
            case MessageBoxButton.YesNo:
                AddButton("No", MessageBoxResult.No, false);
                AddButton("Yes", MessageBoxResult.Yes, true);
                break;
            case MessageBoxButton.YesNoCancel:
                AddButton("Cancel", MessageBoxResult.Cancel, false);
                AddButton("No", MessageBoxResult.No, false);
                AddButton("Yes", MessageBoxResult.Yes, true);
                break;
            default:
                AddButton("OK", MessageBoxResult.OK, true);
                break;
        }
    }

    private void AddButton(string text, MessageBoxResult result, bool primary)
    {
        var button = new Button
        {
            Content = text,
            MinWidth = 96,
            Height = 38,
            Margin = new Thickness(8, 0, 0, 0),
            Style = FindResource(primary ? "PrimaryButtonStyle" : "SecondaryButtonStyle") as Style,
            IsDefault = primary,
            IsCancel = result is MessageBoxResult.Cancel or MessageBoxResult.No
        };

        button.Click += (_, _) =>
        {
            _result = result;
            DialogResult = true;
            Close();
        };

        ButtonsPanel.Children.Add(button);
    }
}
