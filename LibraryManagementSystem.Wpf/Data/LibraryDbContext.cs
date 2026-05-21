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
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Patron> Patrons => Set<Patron>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User entity configuration
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

        // Book entity configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(150);
            entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.Property(b => b.Genre).HasMaxLength(50);
            entity.Property(b => b.Publisher).HasMaxLength(150);
            entity.Property(b => b.Description).HasMaxLength(1000);
            entity.Property(b => b.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // Patron entity configuration
        modelBuilder.Entity<Patron>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FullName).IsRequired().HasMaxLength(150);
            entity.Property(p => p.MembershipId).IsRequired().HasMaxLength(50);
            entity.HasIndex(p => p.MembershipId).IsUnique();
            entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Email).IsUnique();
            entity.Property(p => p.PhoneNumber).HasMaxLength(20);
            entity.Property(p => p.Address).HasMaxLength(300);
            entity.Property(p => p.MembershipType).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // Transaction entity configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.FineAmount).HasColumnType("decimal(10,2)");
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("datetime('now')");

            // Relationships
            entity.HasOne(t => t.Book)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Patron)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PatronId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            entity.HasIndex(t => t.BookId);
            entity.HasIndex(t => t.PatronId);
            entity.HasIndex(t => t.CheckoutDate);
            entity.HasIndex(t => t.IsReturned);
        });

        // Fine entity configuration
        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Amount).HasColumnType("decimal(10,2)");
            entity.Property(f => f.Reason).HasMaxLength(500);
            entity.Property(f => f.CreatedAt).HasDefaultValueSql("datetime('now')");

            // Relationships
            entity.HasOne(f => f.Patron)
                .WithMany(p => p.Fines)
                .HasForeignKey(f => f.PatronId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            entity.HasIndex(f => f.PatronId);
            entity.HasIndex(f => f.IsPaid);
        });

        // Genre entity configuration
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(g => g.Name).IsUnique();
            entity.Property(g => g.CreatedAt).HasDefaultValueSql("datetime('now')");
        });
    }
}
