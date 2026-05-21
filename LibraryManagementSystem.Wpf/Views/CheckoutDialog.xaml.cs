using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views;

public partial class CheckoutDialog : Window
{
    private readonly BookService _bookService;
    private readonly PatronService _patronService;

    public int? SelectedBookId { get; private set; }
    public int? SelectedPatronId { get; private set; }
    public string? SelectedBookTitle { get; private set; }
    public string? SelectedPatronName { get; private set; }

    public CheckoutDialog(BookService bookService, PatronService patronService)
    {
        InitializeComponent();
        _bookService = bookService;
        _patronService = patronService;
        Loaded += CheckoutDialog_Loaded;
    }

    private async void CheckoutDialog_Loaded(object sender, RoutedEventArgs e)
    {
        var patrons = await _patronService.GetActivePatronsAsync();
        PatronCombo.ItemsSource = patrons;
        PatronCombo.SelectedIndex = patrons.Count > 0 ? 0 : -1;

        var books = await _bookService.GetAvailableBooksAsync();
        BookCombo.ItemsSource = books;
        BookCombo.SelectedIndex = books.Count > 0 ? 0 : -1;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (PatronCombo.SelectedItem is not Patron patron || BookCombo.SelectedItem is not Book book)
        {
            ThemedMessageBox.Show("Please select both a patron and a book.", "Checkout", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SelectedPatronId = patron.Id;
        SelectedBookId = book.Id;
        SelectedPatronName = patron.FullName;
        SelectedBookTitle = book.Title;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
