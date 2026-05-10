using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(64);
            entity.Property(u => u.UsernameNormalized).IsRequired().HasMaxLength(64);
            entity.HasIndex(u => u.UsernameNormalized).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
            entity.Property(u => u.PasswordSalt).IsRequired().HasMaxLength(512);
            entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        });
    }
}
