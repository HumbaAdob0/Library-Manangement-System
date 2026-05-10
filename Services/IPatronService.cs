using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IPatronService
    {
        Task<IEnumerable<Patron>> GetAllPatronsAsync();
        Task<Patron?> GetPatronByIdAsync(int id);
        Task<Patron?> GetPatronByMembershipIdAsync(string membershipId);
        Task<IEnumerable<Patron>> SearchPatronsAsync(string searchTerm);
        Task AddPatronAsync(Patron patron);
        Task UpdatePatronAsync(Patron patron);
        Task DeletePatronAsync(int id);
        Task<string> GenerateMembershipIdAsync();
    }
}
