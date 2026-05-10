using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManagementSystem.ViewModels;

public class DashboardCardViewModel
{
    public DashboardCardViewModel(
        string title,
        string subtitle,
        string iconText,
        double width,
        double height,
        Brush accentBrush,
        bool isEnabled,
        ICommand? command = null)
    {
        Title = title;
        Subtitle = subtitle;
        IconText = iconText;
        Width = width;
        Height = height;
        AccentBrush = accentBrush;
        IsEnabled = isEnabled;
        Command = command;
    }

    public string Title { get; }
    public string Subtitle { get; }
    public string IconText { get; }
    public double Width { get; }
    public double Height { get; }
    public Brush AccentBrush { get; }
    public bool IsEnabled { get; }
    public ICommand? Command { get; }
}
