using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class BooksViewModel : ObservableObject
{
    private readonly BookService _bookService;
    private ObservableCollection<Book> _books;
    private Book? _selectedBook;
    private string _searchText = string.Empty;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    public BooksViewModel(BookService bookService)
    {
        _bookService = bookService;
        _books = new ObservableCollection<Book>();

        LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
        SearchCommand = new AsyncRelayCommand(SearchBooksAsync);
        AddBookCommand = new RelayCommand(AddBook);
        EditBookCommand = new RelayCommand(EditBook, () => SelectedBook != null);
        DeleteBookCommand = new AsyncRelayCommand(DeleteBookAsync, () => SelectedBook != null);
        RefreshCommand = new AsyncRelayCommand(LoadBooksAsync);
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set => SetProperty(ref _books, value);
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (SetProperty(ref _selectedBook, value))
            {
                ((RelayCommand)EditBookCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)DeleteBookCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand EditBookCommand { get; }
    public ICommand DeleteBookCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadBooksAsync();
    }

    private async Task LoadBooksAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading books...";

            var books = await _bookService.GetAllBooksAsync();
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }

            StatusMessage = $"Loaded {Books.Count} books";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading books: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchBooksAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadBooksAsync();
                return;
            }

            StatusMessage = "Searching...";
            var books = await _bookService.SearchBooksAsync(SearchText);
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }

            StatusMessage = $"Found {Books.Count} books";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error searching: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AddBook()
    {
        StatusMessage = "Add book feature - Coming soon";
        // TODO: Open add book dialog
    }

    private void EditBook()
    {
        if (SelectedBook == null) return;
        StatusMessage = $"Edit book feature - Coming soon (Selected: {SelectedBook.Title})";
        // TODO: Open edit book dialog
    }

    private async Task DeleteBookAsync()
    {
        if (SelectedBook == null) return;

        try
        {
            var bookTitle = SelectedBook.Title;
            var success = await _bookService.DeleteBookAsync(SelectedBook.Id);

            if (success)
            {
                Books.Remove(SelectedBook);
                StatusMessage = $"Deleted: {bookTitle}";
                SelectedBook = null;
            }
            else
            {
                StatusMessage = "Cannot delete book with active transactions";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting book: {ex.Message}";
        }
    }
}
