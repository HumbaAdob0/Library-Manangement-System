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

    // Add/Edit Dialog Properties
    private bool _isAddEditDialogOpen;
    private bool _isEditMode;
    private string _dialogTitle = string.Empty;
    private string _dialogISBN = string.Empty;
    private string _dialogAuthor = string.Empty;
    private string _dialogGenre = string.Empty;
    private string _dialogPublisher = string.Empty;
    private int _dialogPublishedYear = DateTime.Now.Year;
    private int _dialogTotalCopies = 1;
    private int _dialogAvailableCopies = 1;
    private string _dialogDescription = string.Empty;

    public BooksViewModel(BookService bookService)
    {
        _bookService = bookService;
        _books = new ObservableCollection<Book>();

        LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
        SearchCommand = new AsyncRelayCommand(SearchBooksAsync);
        AddBookCommand = new RelayCommand(OpenAddDialog);
        EditBookCommand = new RelayCommand(OpenEditDialog, () => SelectedBook != null);
        DeleteBookCommand = new AsyncRelayCommand(DeleteBookAsync, () => SelectedBook != null);
        SaveBookCommand = new AsyncRelayCommand(SaveBookAsync);
        CancelDialogCommand = new RelayCommand(CloseDialog);
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

    // Dialog Properties
    public bool IsAddEditDialogOpen
    {
        get => _isAddEditDialogOpen;
        set => SetProperty(ref _isAddEditDialogOpen, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public string DialogTitleText => IsEditMode ? "Edit Book" : "Add New Book";

    public string DialogTitle
    {
        get => _dialogTitle;
        set => SetProperty(ref _dialogTitle, value);
    }

    public string DialogISBN
    {
        get => _dialogISBN;
        set => SetProperty(ref _dialogISBN, value);
    }

    public string DialogAuthor
    {
        get => _dialogAuthor;
        set => SetProperty(ref _dialogAuthor, value);
    }

    public string DialogGenre
    {
        get => _dialogGenre;
        set => SetProperty(ref _dialogGenre, value);
    }

    public string DialogPublisher
    {
        get => _dialogPublisher;
        set => SetProperty(ref _dialogPublisher, value);
    }

    public int DialogPublishedYear
    {
        get => _dialogPublishedYear;
        set => SetProperty(ref _dialogPublishedYear, value);
    }

    public int DialogTotalCopies
    {
        get => _dialogTotalCopies;
        set => SetProperty(ref _dialogTotalCopies, value);
    }

    public int DialogAvailableCopies
    {
        get => _dialogAvailableCopies;
        set => SetProperty(ref _dialogAvailableCopies, value);
    }

    public string DialogDescription
    {
        get => _dialogDescription;
        set => SetProperty(ref _dialogDescription, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand EditBookCommand { get; }
    public ICommand DeleteBookCommand { get; }
    public ICommand SaveBookCommand { get; }
    public ICommand CancelDialogCommand { get; }
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

    private void OpenAddDialog()
    {
        IsEditMode = false;
        DialogTitle = string.Empty;
        DialogISBN = string.Empty;
        DialogAuthor = string.Empty;
        DialogGenre = string.Empty;
        DialogPublisher = string.Empty;
        DialogPublishedYear = DateTime.Now.Year;
        DialogTotalCopies = 1;
        DialogAvailableCopies = 1;
        DialogDescription = string.Empty;
        IsAddEditDialogOpen = true;
        OnPropertyChanged(nameof(DialogTitleText));
    }

    private void OpenEditDialog()
    {
        if (SelectedBook == null) return;

        IsEditMode = true;
        DialogTitle = SelectedBook.Title;
        DialogISBN = SelectedBook.ISBN;
        DialogAuthor = SelectedBook.Author;
        DialogGenre = SelectedBook.Genre;
        DialogPublisher = SelectedBook.Publisher;
        DialogPublishedYear = SelectedBook.PublishedYear;
        DialogTotalCopies = SelectedBook.TotalCopies;
        DialogAvailableCopies = SelectedBook.AvailableCopies;
        DialogDescription = SelectedBook.Description ?? string.Empty;
        IsAddEditDialogOpen = true;
        OnPropertyChanged(nameof(DialogTitleText));
    }

    private void CloseDialog()
    {
        IsAddEditDialogOpen = false;
    }

    private async Task SaveBookAsync()
    {
        try
        {
            // Validation
            if (string.IsNullOrWhiteSpace(DialogTitle))
            {
                StatusMessage = "Title is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(DialogISBN))
            {
                StatusMessage = "ISBN is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(DialogAuthor))
            {
                StatusMessage = "Author is required";
                return;
            }

            if (DialogTotalCopies < 1)
            {
                StatusMessage = "Total copies must be at least 1";
                return;
            }

            if (DialogAvailableCopies < 0 || DialogAvailableCopies > DialogTotalCopies)
            {
                StatusMessage = "Available copies must be between 0 and total copies";
                return;
            }

            if (IsEditMode)
            {
                // Update existing book
                if (SelectedBook == null) return;

                var book = new Book
                {
                    Id = SelectedBook.Id,
                    Title = DialogTitle.Trim(),
                    ISBN = DialogISBN.Trim(),
                    Author = DialogAuthor.Trim(),
                    Genre = DialogGenre.Trim(),
                    Publisher = DialogPublisher.Trim(),
                    PublishedYear = DialogPublishedYear,
                    TotalCopies = DialogTotalCopies,
                    AvailableCopies = DialogAvailableCopies,
                    Description = DialogDescription.Trim(),
                    CreatedAt = SelectedBook.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _bookService.UpdateBookAsync(book);
                if (result != null)
                {
                    StatusMessage = $"Updated book: {result.Title}";
                    // Replace the item in the collection with the returned entity so UI updates immediately
                    var existingIdx = Books.IndexOf(Books.FirstOrDefault(b => b.Id == result.Id));
                    if (existingIdx >= 0)
                    {
                        Books[existingIdx] = result;
                        SelectedBook = result;
                    }

                    CloseDialog();
                }
                else
                {
                    StatusMessage = "Failed to update book";
                }
            }
            else
            {
                // Add new book
                var book = new Book
                {
                    Title = DialogTitle.Trim(),
                    ISBN = DialogISBN.Trim(),
                    Author = DialogAuthor.Trim(),
                    Genre = DialogGenre.Trim(),
                    Publisher = DialogPublisher.Trim(),
                    PublishedYear = DialogPublishedYear,
                    TotalCopies = DialogTotalCopies,
                    AvailableCopies = DialogAvailableCopies,
                    Description = DialogDescription.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _bookService.AddBookAsync(book);
                if (result != null)
                {
                    StatusMessage = $"Added book: {book.Title}";
                    // Add to collection so UI updates immediately
                    Books.Add(result);
                    CloseDialog();
                }
                else
                {
                    StatusMessage = "Failed to add book (ISBN may already exist)";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving book: {ex.Message}";
        }
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
                // Remove by id in case the instances differ
                var toRemove = Books.FirstOrDefault(b => b.Id == SelectedBook.Id);
                if (toRemove != null)
                {
                    Books.Remove(toRemove);
                }

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
