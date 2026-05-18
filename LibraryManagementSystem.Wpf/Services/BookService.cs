using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class BookService
{
    private readonly LibraryDbContext _dbContext;

    public BookService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _dbContext.Books
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _dbContext.Books
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> SearchBooksAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _dbContext.Books
            .Where(b => b.Title.ToLower().Contains(term) ||
                       b.Author.ToLower().Contains(term) ||
                       b.ISBN.Contains(term) ||
                       b.Genre.ToLower().Contains(term))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<List<Book>> GetAvailableBooksAsync()
    {
        return await _dbContext.Books
            .Where(b => b.AvailableCopies > 0)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        book.CreatedAt = DateTime.UtcNow;
        book.AvailableCopies = book.TotalCopies;
        
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        book.UpdatedAt = DateTime.UtcNow;
        
        _dbContext.Books.Update(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
            return false;

        // Check if book has active transactions
        var hasActiveTransactions = await _dbContext.Transactions
            .AnyAsync(t => t.BookId == id && !t.IsReturned);

        if (hasActiveTransactions)
            return false;

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsISBNUniqueAsync(string isbn, int? excludeBookId = null)
    {
        var query = _dbContext.Books.Where(b => b.ISBN == isbn);
        
        if (excludeBookId.HasValue)
            query = query.Where(b => b.Id != excludeBookId.Value);

        return !await query.AnyAsync();
    }
}
