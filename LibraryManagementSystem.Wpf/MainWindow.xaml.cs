using System.Windows;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.SignOutRequested += OnSignOutRequested;
    }

    private void OnSignOutRequested()
    {
        var loginWindow = App.AppHost.Services.GetRequiredService<LoginWindow>();
        Application.Current.MainWindow = loginWindow;
        loginWindow.Show();
        Close();
    }
}
