using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class AuthenticationService
{
    private readonly LibraryDbContext _dbContext;
    private readonly PasswordHasher _passwordHasher;

    public AuthenticationService(LibraryDbContext dbContext, PasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> SignInAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var normalized = username.Trim().ToUpperInvariant();
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UsernameNormalized == normalized && u.IsActive);

        if (user == null)
        {
            return null;
        }

        return _passwordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }
}
