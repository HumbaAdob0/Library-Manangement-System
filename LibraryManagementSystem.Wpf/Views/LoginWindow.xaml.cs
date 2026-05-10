using System.Windows;
using LibraryManagementSystem.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.LoginSucceeded += OnLoginSucceeded;
    }

    private void OnLoginSucceeded()
    {
        var mainWindow = App.AppHost.Services.GetRequiredService<MainWindow>();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
    }
}
