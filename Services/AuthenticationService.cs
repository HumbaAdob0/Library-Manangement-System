using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly LibraryDbContext _context;
        private User? _currentUser;

        public AuthenticationService(LibraryDbContext context)
        {
            _context = context;
        }

        public User? CurrentUser => _currentUser;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _currentUser = user;
                return user;
            }

            return null;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}
