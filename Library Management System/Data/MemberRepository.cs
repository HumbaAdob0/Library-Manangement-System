using Library_Management_System.Models;

namespace Library_Management_System.Data
{
    /// <summary>
    /// Repository for Member data access operations.
    /// </summary>
    public class MemberRepository : IRepository<Member>
    {
        // TODO: Implement database connection and operations
        // This is a placeholder for future implementation

        public Task<IEnumerable<Member>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Member?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Member entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Member entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
