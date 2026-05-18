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
