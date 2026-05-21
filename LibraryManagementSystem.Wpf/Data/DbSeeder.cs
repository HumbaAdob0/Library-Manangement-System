using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Helpers;

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
        SeedUsers();
        SeedGenres();
        SeedBooks();
        SeedGenres();
        NormalizeBookISBNs();
        SeedPatrons();
        SeedTransactions();
    }

    private void SeedUsers()
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

    private void SeedGenres()
    {
        var genreNames = new List<string>
        {
            "Fiction",
            "Non-Fiction",
            "Science Fiction",
            "Fantasy",
            "Mystery",
            "Thriller",
            "Romance",
            "Horror",
            "Biography",
            "History",
            "Self-Help",
            "Poetry",
            "Drama",
            "Adventure",
            "Children",
            "Young Adult",
            "Dystopian",
            "Classic",
            "Educational",
            "Reference"
        };

        genreNames.AddRange(_dbContext.Books
            .Select(b => b.Genre)
            .Where(g => !string.IsNullOrWhiteSpace(g))
            .ToList());

        var existingNames = _dbContext.Genres
            .Select(g => g.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var genresToAdd = genreNames
            .Select(g => g.Trim())
            .Where(g => !string.IsNullOrWhiteSpace(g))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(g => !existingNames.Contains(g))
            .Select(g => new Genre { Name = g, CreatedAt = DateTime.UtcNow })
            .ToList();

        if (genresToAdd.Count > 0)
        {
            _dbContext.Genres.AddRange(genresToAdd);
            _dbContext.SaveChanges();
        }
    }

    private void SeedBooks()
    {
        if (_dbContext.Books.Any())
        {
            return;
        }

        var books = new List<Book>
        {
            new Book
            {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                ISBN = "978-0-06-112008-4",
                Genre = "Fiction",
                Publisher = "J.B. Lippincott & Co.",
                PublishedYear = 1960,
                TotalCopies = 5,
                AvailableCopies = 5,
                Description = "A classic novel depicting racial injustice in the American South.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "1984",
                Author = "George Orwell",
                ISBN = "978-0-452-28423-4",
                Genre = "Dystopian",
                Publisher = "Secker & Warburg",
                PublishedYear = 1949,
                TotalCopies = 4,
                AvailableCopies = 4,
                Description = "A dystopian social science fiction novel and cautionary tale.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                ISBN = "978-0-14-143951-8",
                Genre = "Romance",
                Publisher = "T. Egerton",
                PublishedYear = 1813,
                TotalCopies = 3,
                AvailableCopies = 3,
                Description = "A romantic novel of manners set in Georgian England.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                ISBN = "978-0-7432-7356-5",
                Genre = "Fiction",
                Publisher = "Charles Scribner's Sons",
                PublishedYear = 1925,
                TotalCopies = 6,
                AvailableCopies = 6,
                Description = "A novel about the American Dream in the Roaring Twenties.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                ISBN = "978-0-316-76948-0",
                Genre = "Fiction",
                Publisher = "Little, Brown and Company",
                PublishedYear = 1951,
                TotalCopies = 4,
                AvailableCopies = 4,
                Description = "A story about teenage rebellion and alienation.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "Harry Potter and the Philosopher's Stone",
                Author = "J.K. Rowling",
                ISBN = "978-0-7475-3269-9",
                Genre = "Fantasy",
                Publisher = "Bloomsbury",
                PublishedYear = 1997,
                TotalCopies = 8,
                AvailableCopies = 8,
                Description = "The first novel in the Harry Potter series.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                ISBN = "978-0-547-92822-7",
                Genre = "Fantasy",
                Publisher = "George Allen & Unwin",
                PublishedYear = 1937,
                TotalCopies = 5,
                AvailableCopies = 5,
                Description = "A fantasy novel about the adventures of Bilbo Baggins.",
                CreatedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "The Da Vinci Code",
                Author = "Dan Brown",
                ISBN = "978-0-385-50420-1",
                Genre = "Mystery",
                Publisher = "Doubleday",
                PublishedYear = 2003,
                TotalCopies = 4,
                AvailableCopies = 4,
                Description = "A mystery thriller novel involving art, history, and conspiracy.",
                CreatedAt = DateTime.UtcNow
            }
        };

        _dbContext.Books.AddRange(books);
        _dbContext.SaveChanges();
    }

    private void NormalizeBookISBNs()
    {
        var books = _dbContext.Books.ToList();
        var hasChanges = false;

        foreach (var book in books)
        {
            var formattedISBN = ISBNHelper.FormatISBN13(book.ISBN);
            if (!ISBNHelper.IsValidISBN13(formattedISBN) || book.ISBN == formattedISBN)
            {
                continue;
            }

            var duplicateExists = books.Any(other =>
                other.Id != book.Id &&
                string.Equals(ISBNHelper.FormatISBN13(other.ISBN), formattedISBN, StringComparison.Ordinal));
            if (duplicateExists)
            {
                continue;
            }

            book.ISBN = formattedISBN;
            book.UpdatedAt = DateTime.UtcNow;
            hasChanges = true;
        }

        if (hasChanges)
        {
            _dbContext.SaveChanges();
        }
    }

    private void SeedPatrons()
    {
        if (_dbContext.Patrons.Any())
        {
            return;
        }

        var patrons = new List<Patron>
        {
            new Patron
            {
                FullName = "John Smith",
                MembershipId = "MEM001",
                Email = "john.smith@email.com",
                PhoneNumber = "+1-555-0101",
                Address = "123 Main St, Springfield, IL 62701",
                DateOfBirth = new DateTime(1985, 5, 15),
                MembershipType = MembershipType.Premium,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Patron
            {
                FullName = "Emily Johnson",
                MembershipId = "MEM002",
                Email = "emily.johnson@email.com",
                PhoneNumber = "+1-555-0102",
                Address = "456 Oak Ave, Springfield, IL 62702",
                DateOfBirth = new DateTime(1990, 8, 22),
                MembershipType = MembershipType.Standard,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Patron
            {
                FullName = "Michael Brown",
                MembershipId = "MEM003",
                Email = "michael.brown@email.com",
                PhoneNumber = "+1-555-0103",
                Address = "789 Pine Rd, Springfield, IL 62703",
                DateOfBirth = new DateTime(1978, 3, 10),
                MembershipType = MembershipType.Premium,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Patron
            {
                FullName = "Sarah Davis",
                MembershipId = "MEM004",
                Email = "sarah.davis@email.com",
                PhoneNumber = "+1-555-0104",
                Address = "321 Elm St, Springfield, IL 62704",
                DateOfBirth = new DateTime(1995, 11, 30),
                MembershipType = MembershipType.Standard,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Patron
            {
                FullName = "David Wilson",
                MembershipId = "MEM005",
                Email = "david.wilson@email.com",
                PhoneNumber = "+1-555-0105",
                Address = "654 Maple Dr, Springfield, IL 62705",
                DateOfBirth = new DateTime(1982, 7, 18),
                MembershipType = MembershipType.Standard,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        _dbContext.Patrons.AddRange(patrons);
        _dbContext.SaveChanges();
    }

    private void SeedTransactions()
    {
        if (_dbContext.Transactions.Any())
        {
            return;
        }

        // Add some sample transactions (checked out books)
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                BookId = 1, // To Kill a Mockingbird
                PatronId = 1, // John Smith
                CheckoutDate = DateTime.UtcNow.AddDays(-10),
                DueDate = DateTime.UtcNow.AddDays(4),
                IsReturned = false,
                FineAmount = 0,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Transaction
            {
                BookId = 2, // 1984
                PatronId = 2, // Emily Johnson
                CheckoutDate = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(9),
                IsReturned = false,
                FineAmount = 0,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        _dbContext.Transactions.AddRange(transactions);
        
        // Update available copies
        var book1 = _dbContext.Books.Find(1);
        if (book1 != null) book1.AvailableCopies--;
        
        var book2 = _dbContext.Books.Find(2);
        if (book2 != null) book2.AvailableCopies--;

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
