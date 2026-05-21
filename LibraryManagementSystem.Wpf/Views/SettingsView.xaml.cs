using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views;

public partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        Loaded += async (s, e) => await vm.InitializeAsync();
    }
}
