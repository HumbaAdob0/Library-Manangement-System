using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Views;

public partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
