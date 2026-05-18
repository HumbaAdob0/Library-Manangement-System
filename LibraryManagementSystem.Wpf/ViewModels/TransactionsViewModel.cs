using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class TransactionsViewModel : ObservableObject
{
    private readonly TransactionService _transactionService;
    private ObservableCollection<Transaction> _transactions;
    private Transaction? _selectedTransaction;
    private bool _showActiveOnly = true;
    private bool _showOverdueOnly;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    public TransactionsViewModel(TransactionService transactionService)
    {
        _transactionService = transactionService;
        _transactions = new ObservableCollection<Transaction>();

        LoadTransactionsCommand = new AsyncRelayCommand(LoadTransactionsAsync);
        CheckoutCommand = new RelayCommand(Checkout);
        ReturnCommand = new AsyncRelayCommand(ReturnBookAsync, () => SelectedTransaction != null && !SelectedTransaction.IsReturned);
        ShowAllCommand = new AsyncRelayCommand(ShowAllTransactionsAsync);
        ShowActiveCommand = new AsyncRelayCommand(ShowActiveTransactionsAsync);
        ShowOverdueCommand = new AsyncRelayCommand(ShowOverdueTransactionsAsync);
        RefreshCommand = new AsyncRelayCommand(LoadTransactionsAsync);
    }

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

    private void Checkout()
    {
        StatusMessage = "Checkout feature - Coming soon";
        // TODO: Open checkout dialog
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
