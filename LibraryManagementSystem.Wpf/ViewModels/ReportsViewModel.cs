using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.ViewModels;

public class ReportsViewModel : ObservableObject
{
    private readonly LibraryDbContext _dbContext;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    // Overview Statistics
    private int _totalBooks;
    private int _availableBooks;
    private int _checkedOutBooks;
    private int _totalPatrons;
    private int _activePatrons;
    private int _activeTransactions;
    private int _overdueTransactions;
    private decimal _totalFines;
    private decimal _unpaidFines;

    // Top Lists
    private ObservableCollection<BookStatistic> _mostBorrowedBooks;
    private ObservableCollection<PatronStatistic> _topPatrons;
    private ObservableCollection<Transaction> _recentTransactions;
    private ObservableCollection<Transaction> _overdueBooks;

    // Date Range Filter
    private DateTime _startDate;
    private DateTime _endDate;

    public ReportsViewModel(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
        _mostBorrowedBooks = new ObservableCollection<BookStatistic>();
        _topPatrons = new ObservableCollection<PatronStatistic>();
        _recentTransactions = new ObservableCollection<Transaction>();
        _overdueBooks = new ObservableCollection<Transaction>();

        // Default to last 30 days
        _endDate = DateTime.Today;
        _startDate = _endDate.AddDays(-30);

        LoadReportsCommand = new AsyncRelayCommand(LoadReportsAsync);
        RefreshCommand = new AsyncRelayCommand(LoadReportsAsync);
        ExportCommand = new RelayCommand(ExportReports);
    }

    // Properties
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

    public int TotalBooks
    {
        get => _totalBooks;
        set => SetProperty(ref _totalBooks, value);
    }

    public int AvailableBooks
    {
        get => _availableBooks;
        set => SetProperty(ref _availableBooks, value);
    }

    public int CheckedOutBooks
    {
        get => _checkedOutBooks;
        set => SetProperty(ref _checkedOutBooks, value);
    }

    public int TotalPatrons
    {
        get => _totalPatrons;
        set => SetProperty(ref _totalPatrons, value);
    }

    public int ActivePatrons
    {
        get => _activePatrons;
        set => SetProperty(ref _activePatrons, value);
    }

    public int ActiveTransactions
    {
        get => _activeTransactions;
        set => SetProperty(ref _activeTransactions, value);
    }

    public int OverdueTransactions
    {
        get => _overdueTransactions;
        set => SetProperty(ref _overdueTransactions, value);
    }

    public decimal TotalFines
    {
        get => _totalFines;
        set => SetProperty(ref _totalFines, value);
    }

    public decimal UnpaidFines
    {
        get => _unpaidFines;
        set => SetProperty(ref _unpaidFines, value);
    }

    public ObservableCollection<BookStatistic> MostBorrowedBooks
    {
        get => _mostBorrowedBooks;
        set => SetProperty(ref _mostBorrowedBooks, value);
    }

    public ObservableCollection<PatronStatistic> TopPatrons
    {
        get => _topPatrons;
        set => SetProperty(ref _topPatrons, value);
    }

    public ObservableCollection<Transaction> RecentTransactions
    {
        get => _recentTransactions;
        set => SetProperty(ref _recentTransactions, value);
    }

    public ObservableCollection<Transaction> OverdueBooks
    {
        get => _overdueBooks;
        set => SetProperty(ref _overdueBooks, value);
    }

    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    public ICommand LoadReportsCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ExportCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadReportsAsync();
    }

    private async Task LoadReportsAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading reports...";

            // Load all statistics in parallel
            await Task.WhenAll(
                LoadOverviewStatisticsAsync(),
                LoadMostBorrowedBooksAsync(),
                LoadTopPatronsAsync(),
                LoadRecentTransactionsAsync(),
                LoadOverdueBooksAsync()
            );

            StatusMessage = $"Reports loaded successfully (Period: {StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy})";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading reports: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadOverviewStatisticsAsync()
    {
        // Books Statistics
        TotalBooks = await _dbContext.Books.CountAsync();
        AvailableBooks = await _dbContext.Books.CountAsync(b => b.AvailableCopies > 0);
        CheckedOutBooks = TotalBooks - AvailableBooks;

        // Patrons Statistics
        TotalPatrons = await _dbContext.Patrons.CountAsync();
        ActivePatrons = await _dbContext.Patrons.CountAsync(p => p.IsActive);

        // Transactions Statistics
        ActiveTransactions = await _dbContext.Transactions.CountAsync(t => t.ReturnDate == null);
        OverdueTransactions = await _dbContext.Transactions
            .CountAsync(t => t.ReturnDate == null && t.DueDate < DateTime.Today);

        // Fines Statistics
        TotalFines = await _dbContext.Fines.SumAsync(f => (decimal?)f.Amount) ?? 0;
        UnpaidFines = await _dbContext.Fines
            .Where(f => !f.IsPaid)
            .SumAsync(f => (decimal?)f.Amount) ?? 0;
    }

    private async Task LoadMostBorrowedBooksAsync()
    {
        var bookStats = await _dbContext.Transactions
            .Where(t => t.CheckoutDate >= StartDate && t.CheckoutDate <= EndDate)
            .GroupBy(t => new { t.BookId, t.Book.Title, t.Book.Author })
            .Select(g => new BookStatistic
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                Author = g.Key.Author,
                BorrowCount = g.Count()
            })
            .OrderByDescending(b => b.BorrowCount)
            .Take(10)
            .ToListAsync();

        MostBorrowedBooks.Clear();
        foreach (var stat in bookStats)
        {
            MostBorrowedBooks.Add(stat);
        }
    }

    private async Task LoadTopPatronsAsync()
    {
        var patronStats = await _dbContext.Transactions
            .Where(t => t.CheckoutDate >= StartDate && t.CheckoutDate <= EndDate)
            .GroupBy(t => new { t.PatronId, t.Patron.FullName })
            .Select(g => new PatronStatistic
            {
                PatronId = g.Key.PatronId,
                Name = g.Key.FullName,
                BorrowCount = g.Count()
            })
            .OrderByDescending(p => p.BorrowCount)
            .Take(10)
            .ToListAsync();

        TopPatrons.Clear();
        foreach (var stat in patronStats)
        {
            TopPatrons.Add(stat);
        }
    }

    private async Task LoadRecentTransactionsAsync()
    {
        var transactions = await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .OrderByDescending(t => t.CheckoutDate)
            .Take(10)
            .ToListAsync();

        RecentTransactions.Clear();
        foreach (var transaction in transactions)
        {
            RecentTransactions.Add(transaction);
        }
    }

    private async Task LoadOverdueBooksAsync()
    {
        var overdueTransactions = await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .Where(t => t.ReturnDate == null && t.DueDate < DateTime.Today)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        OverdueBooks.Clear();
        foreach (var transaction in overdueTransactions)
        {
            OverdueBooks.Add(transaction);
        }
    }

    private void ExportReports()
    {
        StatusMessage = "Export feature coming soon...";
    }
}

public class BookStatistic
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}

public class PatronStatistic
{
    public int PatronId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}
