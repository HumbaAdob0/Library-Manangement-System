using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Views;

namespace LibraryManagementSystem.ViewModels;

public class TransactionsViewModel : ObservableObject
{
    private const string ActiveFilter = "Active";
    private const string AllFilter = "All";
    private const string ReturnedFilter = "Returned";
    private const string OverdueFilter = "Overdue";

    private readonly TransactionService _transactionService;
    private readonly BookService _bookService;
    private readonly PatronService _patronService;
    private ObservableCollection<Transaction> _transactions;
    private ObservableCollection<Book> _availableBooks;
    private ObservableCollection<Patron> _availablePatrons;
    private Transaction? _selectedTransaction;
    private string _selectedFilter = AllFilter;
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private bool _isEditDialogOpen;
    private int? _dialogBookId;
    private int? _dialogPatronId;
    private DateTime? _dialogCheckoutDate;
    private DateTime? _dialogDueDate;
    private DateTime? _dialogReturnDate;
    private decimal _dialogFineAmount;
    private bool _isSelectedTransactionReturned;

    public TransactionsViewModel(TransactionService transactionService, BookService bookService, PatronService patronService)
    {
        _transactionService = transactionService;
        _bookService = bookService;
        _patronService = patronService;
        _transactions = new ObservableCollection<Transaction>();
        _availableBooks = new ObservableCollection<Book>();
        _availablePatrons = new ObservableCollection<Patron>();

        FilterOptions = new ObservableCollection<string>
        {
            AllFilter,
            ActiveFilter,
            ReturnedFilter,
            OverdueFilter
        };

        LoadTransactionsCommand = new AsyncRelayCommand(LoadTransactionsAsync);
        CheckoutCommand = new AsyncRelayCommand(CheckoutAsync);
        EditTransactionCommand = new AsyncRelayCommand(OpenEditDialogAsync, () => SelectedTransaction != null);
        ReturnCommand = new AsyncRelayCommand(ReturnBookAsync, () => SelectedTransaction != null && !SelectedTransaction.IsReturned);
        SaveTransactionCommand = new AsyncRelayCommand(SaveTransactionAsync);
        CancelEditDialogCommand = new RelayCommand(CloseEditDialog);
        RefreshCommand = new AsyncRelayCommand(LoadTransactionsAsync);
    }

    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public ObservableCollection<Book> AvailableBooks
    {
        get => _availableBooks;
        set => SetProperty(ref _availableBooks, value);
    }

    public ObservableCollection<Patron> AvailablePatrons
    {
        get => _availablePatrons;
        set => SetProperty(ref _availablePatrons, value);
    }

    public ObservableCollection<string> FilterOptions { get; }

    public Transaction? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            if (SetProperty(ref _selectedTransaction, value))
            {
                EditTransactionCommand.RaiseCanExecuteChanged();
                ReturnCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            if (SetProperty(ref _selectedFilter, value))
            {
                _ = LoadTransactionsAsync();
            }
        }
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

    public bool IsEditDialogOpen
    {
        get => _isEditDialogOpen;
        set => SetProperty(ref _isEditDialogOpen, value);
    }

    public int? DialogBookId
    {
        get => _dialogBookId;
        set => SetProperty(ref _dialogBookId, value);
    }

    public int? DialogPatronId
    {
        get => _dialogPatronId;
        set => SetProperty(ref _dialogPatronId, value);
    }

    public DateTime? DialogCheckoutDate
    {
        get => _dialogCheckoutDate;
        set => SetProperty(ref _dialogCheckoutDate, value);
    }

    public DateTime? DialogDueDate
    {
        get => _dialogDueDate;
        set => SetProperty(ref _dialogDueDate, value);
    }

    public DateTime? DialogReturnDate
    {
        get => _dialogReturnDate;
        set => SetProperty(ref _dialogReturnDate, value);
    }

    public decimal DialogFineAmount
    {
        get => _dialogFineAmount;
        set => SetProperty(ref _dialogFineAmount, value);
    }

    public bool IsSelectedTransactionReturned
    {
        get => _isSelectedTransactionReturned;
        set => SetProperty(ref _isSelectedTransactionReturned, value);
    }

    public ICommand LoadTransactionsCommand { get; }
    public ICommand CheckoutCommand { get; }
    public AsyncRelayCommand EditTransactionCommand { get; }
    public AsyncRelayCommand ReturnCommand { get; }
    public ICommand SaveTransactionCommand { get; }
    public ICommand CancelEditDialogCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadLookupDataAsync();
        await LoadTransactionsAsync();
    }

    private async Task LoadLookupDataAsync()
    {
        var books = await _bookService.GetAllBooksAsync();
        AvailableBooks.Clear();
        foreach (var book in books)
        {
            AvailableBooks.Add(book);
        }

        var patrons = await _patronService.GetAllPatronsAsync();
        AvailablePatrons.Clear();
        foreach (var patron in patrons)
        {
            AvailablePatrons.Add(patron);
        }
    }

    private async Task LoadTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = $"Loading {SelectedFilter.ToLowerInvariant()} transactions...";

            var transactions = SelectedFilter switch
            {
                AllFilter => await _transactionService.GetAllTransactionsAsync(),
                ReturnedFilter => await _transactionService.GetReturnedTransactionsAsync(),
                OverdueFilter => await _transactionService.GetOverdueTransactionsAsync(),
                _ => await _transactionService.GetActiveTransactionsAsync()
            };

            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            StatusMessage = $"Loaded {Transactions.Count} {SelectedFilter.ToLowerInvariant()} transactions";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading transactions: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task CheckoutAsync()
    {
        try
        {
            var dialog = new CheckoutDialog(_bookService, _patronService)
            {
                Owner = Application.Current?.MainWindow
            };
            var showResult = dialog.ShowDialog();
            if (showResult != true)
            {
                StatusMessage = "Checkout cancelled";
                return;
            }

            if (!dialog.SelectedBookId.HasValue || !dialog.SelectedPatronId.HasValue)
            {
                MessageBox.Show("Please select both a book and a patron.", "Checkout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var bookId = dialog.SelectedBookId.Value;
            var patronId = dialog.SelectedPatronId.Value;

            await _transactionService.CheckoutBookAsync(bookId, patronId);
            await LoadLookupDataAsync();
            await LoadTransactionsAsync();

            var bookTitle = dialog.SelectedBookTitle ?? "Book";
            var patronName = dialog.SelectedPatronName ?? "Patron";
            StatusMessage = $"Checked out '{bookTitle}' to {patronName}";

            MessageBox.Show($"Checked out '{bookTitle}' to '{patronName}'.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Checkout failed: {ex.Message}";
            MessageBox.Show(ex.Message, "Checkout Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task OpenEditDialogAsync()
    {
        if (SelectedTransaction == null)
        {
            return;
        }

        try
        {
            await LoadLookupDataAsync();

            var transaction = SelectedTransaction;
            DialogBookId = transaction.BookId;
            DialogPatronId = transaction.PatronId;
            DialogCheckoutDate = transaction.CheckoutDate.Date;
            DialogDueDate = transaction.DueDate.Date;
            DialogReturnDate = transaction.ReturnDate?.Date;
            DialogFineAmount = transaction.FineAmount;
            IsSelectedTransactionReturned = transaction.IsReturned;
            IsEditDialogOpen = true;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error opening transaction editor: {ex.Message}";
        }
    }

    private void CloseEditDialog()
    {
        IsEditDialogOpen = false;
    }

    private async Task SaveTransactionAsync()
    {
        if (SelectedTransaction == null)
        {
            return;
        }

        try
        {
            if (!DialogBookId.HasValue)
            {
                StatusMessage = "Book is required";
                return;
            }

            if (!DialogPatronId.HasValue)
            {
                StatusMessage = "Patron is required";
                return;
            }

            if (!DialogCheckoutDate.HasValue || !DialogDueDate.HasValue)
            {
                StatusMessage = "Checkout date and due date are required";
                return;
            }

            if (DialogDueDate.Value.Date < DialogCheckoutDate.Value.Date)
            {
                StatusMessage = "Due date cannot be before checkout date";
                return;
            }

            if (IsSelectedTransactionReturned)
            {
                if (!DialogReturnDate.HasValue)
                {
                    StatusMessage = "Return date is required";
                    return;
                }

                if (DialogReturnDate.Value.Date < DialogCheckoutDate.Value.Date)
                {
                    StatusMessage = "Return date cannot be before checkout date";
                    return;
                }
            }

            if (DialogFineAmount < 0)
            {
                StatusMessage = "Fine cannot be negative";
                return;
            }

            var updatedTransaction = await _transactionService.UpdateTransactionAsync(
                SelectedTransaction.Id,
                DialogBookId.Value,
                DialogPatronId.Value,
                DialogCheckoutDate.Value.Date,
                DialogDueDate.Value.Date,
                IsSelectedTransactionReturned ? DialogReturnDate?.Date : null,
                DialogFineAmount);

            StatusMessage = "Transaction updated successfully";
            CloseEditDialog();
            await LoadLookupDataAsync();
            await LoadTransactionsAsync();
            SelectedTransaction = Transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error updating transaction: {ex.Message}";
            MessageBox.Show(ex.Message, "Edit Transaction", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async Task ReturnBookAsync()
    {
        if (SelectedTransaction == null || SelectedTransaction.IsReturned) return;

        try
        {
            var transaction = await _transactionService.ReturnBookAsync(SelectedTransaction.Id);

            if (transaction.FineAmount > 0)
            {
                StatusMessage = $"Book returned. Fine applied: ${transaction.FineAmount:F2}";
            }
            else
            {
                StatusMessage = "Book returned successfully";
            }

            await LoadLookupDataAsync();
            await LoadTransactionsAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error returning book: {ex.Message}";
        }
    }
}
