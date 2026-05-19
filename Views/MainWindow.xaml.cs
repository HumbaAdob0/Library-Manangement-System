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
            ContentFrame.Content = new DashboardView();
        }

        private void ShowBooks()
        {
            // Load the BooksView user control into the content frame
            ContentFrame.Content = new BooksView();
        }

        private void ShowPatrons()
        {
            // Load the PatronsView user control into the content frame
            ContentFrame.Content = new PatronsView();
        }

        private void ShowTransactions()
        {
            // Load the TransactionsView user control into the content frame
            ContentFrame.Content = new TransactionsView();
        }

        private void ShowReports()
        {
            // Load the ReportsView user control into the content frame
            ContentFrame.Content = new ReportsView();
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
