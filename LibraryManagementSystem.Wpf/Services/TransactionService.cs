using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class TransactionService
{
    private readonly LibraryDbContext _dbContext;
    private const decimal DailyFineRate = 0.50m; // $0.50 per day overdue

    public TransactionService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int id)
    {
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Transaction>> GetActiveTransactionsAsync()
    {
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .Where(t => !t.IsReturned)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetOverdueTransactionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .Where(t => !t.IsReturned && t.DueDate < today)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetReturnedTransactionsAsync()
    {
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .Where(t => t.IsReturned)
            .OrderByDescending(t => t.ReturnDate)
            .ThenByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetPatronTransactionsAsync(int patronId)
    {
        return await _dbContext.Transactions
            .Include(t => t.Book)
            .Where(t => t.PatronId == patronId)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetBookTransactionsAsync(int bookId)
    {
        return await _dbContext.Transactions
            .Include(t => t.Patron)
            .Where(t => t.BookId == bookId)
            .OrderByDescending(t => t.CheckoutDate)
            .ToListAsync();
    }

    public async Task<Transaction> CheckoutBookAsync(int bookId, int patronId, int borrowDays = 14)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("No copies available for checkout.");

        var transaction = new Transaction
        {
            BookId = bookId,
            PatronId = patronId,
            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(borrowDays),
            IsReturned = false,
            FineAmount = 0,
            CreatedAt = DateTime.UtcNow
        };

        book.AvailableCopies--;
        
        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();

        return transaction;
    }

    public async Task<Transaction> ReturnBookAsync(int transactionId)
    {
        var transaction = await _dbContext.Transactions
            .Include(t => t.Book)
            .Include(t => t.Patron)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
            throw new InvalidOperationException("Transaction not found.");

        if (transaction.IsReturned)
            throw new InvalidOperationException("Book already returned.");

        transaction.ReturnDate = DateTime.UtcNow;
        transaction.IsReturned = true;
        transaction.UpdatedAt = DateTime.UtcNow;

        // Calculate fine if overdue
        if (transaction.ReturnDate > transaction.DueDate)
        {
            var overdueDays = (transaction.ReturnDate.Value.Date - transaction.DueDate.Date).Days;
            transaction.FineAmount = overdueDays * DailyFineRate;

            // Create fine record
            if (transaction.FineAmount > 0)
            {
                var fine = new Fine
                {
                    PatronId = transaction.PatronId,
                    Amount = transaction.FineAmount,
                    DateApplied = DateTime.UtcNow,
                    IsPaid = false,
                    Reason = $"Overdue return: {overdueDays} days late",
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Fines.Add(fine);
            }
        }

        transaction.Book.AvailableCopies++;
        
        await _dbContext.SaveChangesAsync();

        return transaction;
    }

    public async Task<Transaction> UpdateTransactionAsync(
        int transactionId,
        int bookId,
        int patronId,
        DateTime checkoutDate,
        DateTime dueDate,
        DateTime? returnDate,
        decimal fineAmount)
    {
        var transaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
            throw new InvalidOperationException("Transaction not found.");

        var patronExists = await _dbContext.Patrons.AnyAsync(p => p.Id == patronId);
        if (!patronExists)
            throw new InvalidOperationException("Patron not found.");

        var oldBook = await _dbContext.Books.FindAsync(transaction.BookId);
        var newBook = await _dbContext.Books.FindAsync(bookId);

        if (oldBook == null)
            throw new InvalidOperationException("Original book not found.");

        if (newBook == null)
            throw new InvalidOperationException("Book not found.");

        if (dueDate.Date < checkoutDate.Date)
            throw new InvalidOperationException("Due date cannot be before checkout date.");

        if (transaction.IsReturned)
        {
            if (!returnDate.HasValue)
                throw new InvalidOperationException("Return date is required for returned transactions.");

            if (returnDate.Value.Date < checkoutDate.Date)
                throw new InvalidOperationException("Return date cannot be before checkout date.");
        }

        if (fineAmount < 0)
            throw new InvalidOperationException("Fine cannot be negative.");

        var isActiveTransaction = !transaction.IsReturned;
        var bookChanged = transaction.BookId != bookId;

        if (isActiveTransaction && bookChanged)
        {
            if (newBook.AvailableCopies <= 0)
                throw new InvalidOperationException("The selected book has no available copies.");

            oldBook.AvailableCopies++;
            newBook.AvailableCopies--;
        }

        transaction.BookId = bookId;
        transaction.PatronId = patronId;
        transaction.CheckoutDate = checkoutDate;
        transaction.DueDate = dueDate;
        transaction.ReturnDate = transaction.IsReturned ? returnDate : null;
        transaction.FineAmount = fineAmount;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return await GetTransactionByIdAsync(transaction.Id)
            ?? throw new InvalidOperationException("Transaction could not be loaded after update.");
    }

    public async Task<int> GetActiveTransactionCountAsync(int patronId)
    {
        return await _dbContext.Transactions
            .CountAsync(t => t.PatronId == patronId && !t.IsReturned);
    }

    public async Task<bool> HasOverdueBooks(int patronId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbContext.Transactions
            .AnyAsync(t => t.PatronId == patronId && !t.IsReturned && t.DueDate < today);
    }

    public decimal CalculateOverdueFine(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate)
            return 0;

        var overdueDays = (returnDate.Date - dueDate.Date).Days;
        return overdueDays * DailyFineRate;
    }
}
