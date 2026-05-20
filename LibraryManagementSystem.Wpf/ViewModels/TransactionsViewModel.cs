using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class TransactionsViewModel : ObservableObject
{
    private readonly TransactionService _transactionService;
    private readonly PatronService _patronService;
    private readonly BookService _bookService;
    private ObservableCollection<Transaction> _transactions;
    private Transaction? _selectedTransaction;
    private bool _showActiveOnly = true;
    private bool _showOverdueOnly;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    public TransactionsViewModel(TransactionService transactionService, PatronService patronService, BookService bookService)
    {
        _transactionService = transactionService;
        _patronService = patronService;
        _bookService = bookService;
        _transactions = new ObservableCollection<Transaction>();

        LoadTransactionsCommand = new AsyncRelayCommand(LoadTransactionsAsync);
        CheckoutCommand = new RelayCommand(OpenCheckoutDialog);
        ReturnCommand = new AsyncRelayCommand(ReturnBookAsync, () => SelectedTransaction != null && !SelectedTransaction.IsReturned);
        ShowAllCommand = new AsyncRelayCommand(ShowAllTransactionsAsync);
        ShowActiveCommand = new AsyncRelayCommand(ShowActiveTransactionsAsync);
        ShowOverdueCommand = new AsyncRelayCommand(ShowOverdueTransactionsAsync);
        RefreshCommand = new AsyncRelayCommand(LoadTransactionsAsync);
    }

    // Available patrons and books for checkout dialog
    private ObservableCollection<Patron> _availablePatrons = new();
    private ObservableCollection<Book> _availableBooks = new();

    public ObservableCollection<Patron> AvailablePatrons { get => _availablePatrons; set => SetProperty(ref _availablePatrons, value); }
    public ObservableCollection<Book> AvailableBooks { get => _availableBooks; set => SetProperty(ref _availableBooks, value); }

    // Checkout dialog properties
    private bool _isCheckoutDialogOpen;
    private Patron? _selectedPatronForCheckout;
    private Book? _selectedBookForCheckout;
    private DateTime _checkoutDate = DateTime.Now;
    private DateTime _dueDate = DateTime.Now.AddDays(14);

    public bool IsCheckoutDialogOpen { get => _isCheckoutDialogOpen; set => SetProperty(ref _isCheckoutDialogOpen, value); }
    public Patron? SelectedPatronForCheckout { get => _selectedPatronForCheckout; set => SetProperty(ref _selectedPatronForCheckout, value); }
    public Book? SelectedBookForCheckout { get => _selectedBookForCheckout; set => SetProperty(ref _selectedBookForCheckout, value); }
    public DateTime CheckoutDate { get => _checkoutDate; set => SetProperty(ref _checkoutDate, value); }
    public DateTime DueDate { get => _dueDate; set => SetProperty(ref _dueDate, value); }

    public ICommand SaveCheckoutCommand => new AsyncRelayCommand(SaveCheckoutAsync);
    public ICommand CancelCheckoutCommand => new RelayCommand(CloseCheckoutDialog);



    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public Transaction? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            if (SetProperty(ref _selectedTransaction, value))
            {
                ((AsyncRelayCommand)ReturnCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool ShowActiveOnly
    {
        get => _showActiveOnly;
        set => SetProperty(ref _showActiveOnly, value);
    }

    public bool ShowOverdueOnly
    {
        get => _showOverdueOnly;
        set => SetProperty(ref _showOverdueOnly, value);
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

    public ICommand LoadTransactionsCommand { get; }
    public ICommand CheckoutCommand { get; }
    public ICommand ReturnCommand { get; }
    public ICommand ShowAllCommand { get; }
    public ICommand ShowActiveCommand { get; }
    public ICommand ShowOverdueCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task InitializeAsync()
    {
        await ShowActiveTransactionsAsync();
    }

    private async Task LoadTransactionsAsync()
    {
        if (ShowOverdueOnly)
        {
            await ShowOverdueTransactionsAsync();
        }
        else if (ShowActiveOnly)
        {
            await ShowActiveTransactionsAsync();
        }
        else
        {
            await ShowAllTransactionsAsync();
        }
    }

    private async Task ShowAllTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            ShowActiveOnly = false;
            ShowOverdueOnly = false;
            StatusMessage = "Loading all transactions...";

            var transactions = await _transactionService.GetAllTransactionsAsync();
            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            StatusMessage = $"Loaded {Transactions.Count} transactions";
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

    private async Task ShowActiveTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            ShowActiveOnly = true;
            ShowOverdueOnly = false;
            StatusMessage = "Loading active transactions...";

            var transactions = await _transactionService.GetActiveTransactionsAsync();
            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            StatusMessage = $"Loaded {Transactions.Count} active transactions";
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

    private async Task ShowOverdueTransactionsAsync()
    {
        try
        {
            IsLoading = true;
            ShowActiveOnly = false;
            ShowOverdueOnly = true;
            StatusMessage = "Loading overdue transactions...";

            var transactions = await _transactionService.GetOverdueTransactionsAsync();
            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            StatusMessage = $"Loaded {Transactions.Count} overdue transactions";
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

    private async void OpenCheckoutDialog()
    {
        try
        {
            // Load available patrons and books
            var patrons = await _patronService.GetActivePatronsAsync();
            AvailablePatrons.Clear();
            foreach (var p in patrons) AvailablePatrons.Add(p);

            var books = await _bookService.GetAvailableBooksAsync();
            AvailableBooks.Clear();
            foreach (var b in books) AvailableBooks.Add(b);

            CheckoutDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(14);
            SelectedPatronForCheckout = AvailablePatrons.FirstOrDefault();
            SelectedBookForCheckout = AvailableBooks.FirstOrDefault();
            IsCheckoutDialogOpen = true;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error opening checkout: {ex.Message}";
        }
    }

    private void CloseCheckoutDialog()
    {
        IsCheckoutDialogOpen = false;
    }

    private async Task SaveCheckoutAsync()
    {
        try
        {
            if (SelectedPatronForCheckout == null || SelectedBookForCheckout == null)
            {
                StatusMessage = "Select a patron and a book";
                return;
            }

            var borrowDays = (DueDate.Date - CheckoutDate.Date).Days;
            if (borrowDays < 1) borrowDays = 14;

            var transaction = await _transactionService.CheckoutBookAsync(SelectedBookForCheckout.Id, SelectedPatronForCheckout.Id, borrowDays);

            // Refresh lists
            IsCheckoutDialogOpen = false;
            await LoadTransactionsAsync();
            StatusMessage = $"Checked out '{SelectedBookForCheckout.Title}' to {SelectedPatronForCheckout.FullName}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error during checkout: {ex.Message}";
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

            await LoadTransactionsAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error returning book: {ex.Message}";
        }
    }
}
