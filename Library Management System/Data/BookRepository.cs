using Library_Management_System.Models;

namespace Library_Management_System.Data
{
    /// <summary>
    /// Repository for Book data access operations.
    /// </summary>
    public class BookRepository : IRepository<Book>
    {
        // TODO: Implement database connection and operations
        // This is a placeholder for future implementation

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
