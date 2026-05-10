using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    /// <summary>
    /// Database context for the Library Management System.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Genre).HasMaxLength(100);
                entity.Property(e => e.Publisher).HasMaxLength(200);
                entity.HasIndex(e => e.ISBN).IsUnique();
            });

            // Configure Patron entity
            modelBuilder.Entity<Patron>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MembershipId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.HasIndex(e => e.MembershipId).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FineAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(50);
                
                entity.HasOne(e => e.Book)
                    .WithMany(b => b.Transactions)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Patron)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(e => e.PatronId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Fine entity
            modelBuilder.Entity<Fine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Reason).HasMaxLength(500);
                
                entity.HasOne(e => e.Patron)
                    .WithMany(p => p.Fines)
                    .HasForeignKey(e => e.PatronId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    FullName = "System Administrator",
                    Email = "admin@library.com",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
