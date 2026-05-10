using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchTerm) ||
                           b.Author.Contains(searchTerm) ||
                           b.ISBN.Contains(searchTerm) ||
                           b.Genre.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAvailabilityAsync(bool? isAvailable)
        {
            if (!isAvailable.HasValue)
                return await GetAllBooksAsync();

            return await _context.Books
                .Where(b => isAvailable.Value ? b.AvailableQuantity > 0 : b.AvailableQuantity == 0)
                .ToListAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            book.CreatedDate = DateTime.Now;
            book.AvailableQuantity = book.Quantity;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            return book != null && book.AvailableQuantity > 0;
        }
    }
}

