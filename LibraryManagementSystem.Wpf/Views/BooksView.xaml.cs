using System.Windows.Controls;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views;

public partial class BooksView : UserControl
{
    private bool _isFormattingISBN;

    public BooksView(BooksViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (s, e) => await viewModel.InitializeAsync();
    }

    private void ISBNTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isFormattingISBN || sender is not TextBox textBox)
        {
            return;
        }

        var originalText = textBox.Text;
        var formattedText = ISBNHelper.FormatISBN13(originalText);
        if (originalText == formattedText)
        {
            return;
        }

        var originalCaret = textBox.SelectionStart;

        _isFormattingISBN = true;
        textBox.Text = formattedText;
        textBox.SelectionStart = ISBNHelper.GetCursorPosition(originalText, formattedText, originalCaret);
        _isFormattingISBN = false;
    }
}
