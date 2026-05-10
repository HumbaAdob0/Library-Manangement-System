using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(int userId, string newPassword);
        User? CurrentUser { get; }
        void Logout();
    }
}
