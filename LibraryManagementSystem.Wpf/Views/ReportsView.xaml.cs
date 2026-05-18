using System.Windows.Controls;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views;

public partial class ReportsView : UserControl
{
    public ReportsView(ReportsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (s, e) => await viewModel.InitializeAsync();
    }
}
