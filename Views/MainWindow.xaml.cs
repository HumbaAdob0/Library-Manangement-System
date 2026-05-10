using System.Windows;
using System.Windows.Controls;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authService;

        public MainWindow(MainViewModel viewModel, IAuthenticationService authService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _authService = authService;
            
            // Set default selection
            NavigationListBox.SelectedIndex = 0;
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationListBox.SelectedIndex < 0) return;

            // Navigate to different pages based on selection
            switch (NavigationListBox.SelectedIndex)
            {
                case 0: // Dashboard
                    ShowDashboard();
                    break;
                case 1: // Books
                    ShowBooks();
                    break;
                case 2: // Patrons
                    ShowPatrons();
                    break;
                case 3: // Transactions
                    ShowTransactions();
                    break;
                case 4: // Reports
                    ShowReports();
                    break;
            }
        }

        private void ShowDashboard()
        {
            ContentFrame.Content = new TextBlock 
            { 
                Text = "Dashboard - Coming Soon", 
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowBooks()
        {
            ContentFrame.Content = new TextBlock 
            { 
                Text = "Book Management - Coming Soon", 
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowPatrons()
        {
            ContentFrame.Content = new TextBlock 
            { 
                Text = "Patron Management - Coming Soon", 
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowTransactions()
        {
            ContentFrame.Content = new TextBlock 
            { 
                Text = "Transaction Management - Coming Soon", 
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowReports()
        {
            ContentFrame.Content = new TextBlock 
            { 
                Text = "Reports - Coming Soon", 
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _authService.Logout();
            
            var loginWindow = App.GetService<LoginWindow>();
            loginWindow.Show();
            
            this.Close();
        }
    }
}
