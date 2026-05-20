using System.Windows;
using System.Windows.Controls;
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
        _viewModel.NavigationRequested += OnNavigationRequested;
        // Initialize title from settings
        var settingsService = App.AppHost.Services.GetRequiredService<Services.SettingsService>();
        this.Title = settingsService.Get().LibraryName;
    }

    private void OnSignOutRequested()
    {
        var loginWindow = App.AppHost.Services.GetRequiredService<LoginWindow>();
        Application.Current.MainWindow = loginWindow;
        loginWindow.Show();
        Close();
    }

    private void OnNavigationRequested(string viewName)
    {
        UserControl? view = viewName switch
        {
            "Overview" => App.AppHost.Services.GetRequiredService<ReportsView>(),
            "Books" => App.AppHost.Services.GetRequiredService<BooksView>(),
            "Patrons" => App.AppHost.Services.GetRequiredService<PatronsView>(),
            "Transactions" => App.AppHost.Services.GetRequiredService<TransactionsView>(),
            "Users" => App.AppHost.Services.GetRequiredService<UsersView>(),
            "Settings" => App.AppHost.Services.GetRequiredService<SettingsView>(),
            _ => null
        };

        if (view != null)
        {
            _viewModel.CurrentView = view;
        }
    }
}
