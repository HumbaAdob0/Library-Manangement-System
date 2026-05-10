using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly LibraryDbContext _context;
        private const decimal FinePerDay = 5.0m; // $5 per day overdue

        public TransactionService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CheckoutBookAsync(int bookId, int patronId, int loanDays = 14)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.AvailableQuantity <= 0)
                throw new InvalidOperationException("Book is not available for checkout.");

            var transaction = new Transaction
            {
                BookId = bookId,
                PatronId = patronId,
                CheckoutDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(loanDays),
                Status = "Checked Out"
            };

            book.AvailableQuantity--;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> ReturnBookAsync(int transactionId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
                throw new InvalidOperationException("Transaction not found.");

            transaction.ReturnDate = DateTime.Now;
            transaction.Status = "Returned";

            // Calculate fine if overdue
            if (DateTime.Now > transaction.DueDate)
            {
                transaction.FineAmount = await CalculateFineAsync(transactionId);
                transaction.Status = "Returned with Fine";
            }

            transaction.Book.AvailableQuantity++;
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Patron)
                .OrderByDescending(t => t.CheckoutDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByPatronAsync(int patronId)
        {
            return await _context.Transactions
                .Include(t => t.Book)
                .Where(t => t.PatronId == patronId)
                .OrderByDescending(t => t.CheckoutDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetOverdueTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Patron)
                .Where(t => t.ReturnDate == null && t.DueDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetActiveTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Patron)
                .Where(t => t.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<decimal> CalculateFineAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null || transaction.ReturnDate == null)
                return 0;

            var returnDate = transaction.ReturnDate.Value;
            if (returnDate <= transaction.DueDate)
                return 0;

            var overdueDays = (returnDate - transaction.DueDate).Days;
            return overdueDays * FinePerDay;
        }
    }
}
