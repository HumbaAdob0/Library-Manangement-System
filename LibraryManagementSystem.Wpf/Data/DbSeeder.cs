using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Data;

public class DbSeeder
{
    private readonly LibraryDbContext _dbContext;
    private readonly PasswordHasher _passwordHasher;

    public DbSeeder(LibraryDbContext dbContext, PasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void SeedDefaults()
    {
        if (_dbContext.Users.Any())
        {
            return;
        }

        var admin = CreateUser("admin", "Admin@123", UserRole.Admin);
        var librarian = CreateUser("librarian", "Librarian@123", UserRole.Librarian);

        _dbContext.Users.AddRange(admin, librarian);
        _dbContext.SaveChanges();
    }

    private User CreateUser(string username, string password, UserRole role)
    {
        var normalized = username.Trim().ToUpperInvariant();
        var hashResult = _passwordHasher.HashPassword(password);

        return new User
        {
            Username = username.Trim(),
            UsernameNormalized = normalized,
            PasswordHash = hashResult.Hash,
            PasswordSalt = hashResult.Salt,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}
