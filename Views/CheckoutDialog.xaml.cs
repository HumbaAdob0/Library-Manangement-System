using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class CheckoutDialog : Window
    {
        private readonly IBookService _bookService;
        private readonly IPatronService _patronService;

        public int? SelectedBookId { get; private set; }
        public int? SelectedPatronId { get; private set; }

        public CheckoutDialog(IBookService bookService, IPatronService patronService)
        {
            InitializeComponent();
            _bookService = bookService;
            _patronService = patronService;

            Loaded += CheckoutDialog_Loaded;
        }

        private async void CheckoutDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var patrons = await _patronService.GetAllPatronsAsync();
            PatronCombo.ItemsSource = patrons.OrderBy(p => p.FullName).ToList();

            var books = await _bookService.GetAllBooksAsync();
            // Only show available books
            BookCombo.ItemsSource = books.Where(b => b.AvailableQuantity > 0).OrderBy(b => b.Title).ToList();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (PatronCombo.SelectedValue == null || BookCombo.SelectedValue == null)
            {
                MessageBox.Show("Please select both a patron and a book.", "Checkout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedPatronId = (int)PatronCombo.SelectedValue;
            SelectedBookId = (int)BookCombo.SelectedValue;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
