# Service Usage Guide

Quick reference for using the backend services in your ViewModels.

## Dependency Injection

All services are registered and can be injected into ViewModels:

```csharp
public class BooksViewModel
{
    private readonly BookService _bookService;
    
    public BooksViewModel(BookService bookService)
    {
        _bookService = bookService;
    }
}
```

## Common Usage Patterns

### 1. Loading Data

```csharp
// Load all books
var books = await _bookService.GetAllBooksAsync();

// Load all patrons
var patrons = await _patronService.GetAllPatronsAsync();

// Load active transactions
var activeTransactions = await _transactionService.GetActiveTransactionsAsync();

// Load overdue transactions
var overdueTransactions = await _transactionService.GetOverdueTransactionsAsync();
```

### 2. Searching

```csharp
// Search books by title, author, ISBN, or genre
var searchResults = await _bookService.SearchBooksAsync("Harry Potter");

// Search patrons by name, membership ID, or email
var patronResults = await _patronService.SearchPatronsAsync("john");

// Get available books only
var availableBooks = await _bookService.GetAvailableBooksAsync();
```

### 3. Adding New Records

```csharp
// Add a new book
var newBook = new Book
{
    Title = "The Hobbit",
    Author = "J.R.R. Tolkien",
    ISBN = "978-0547928227",
    Genre = "Fantasy",
    Publisher = "Houghton Mifflin",
    PublishedYear = 1937,
    TotalCopies = 5,
    Description = "A fantasy adventure novel"
};
var addedBook = await _bookService.AddBookAsync(newBook);

// Add a new patron
var newPatron = new Patron
{
    FullName = "Jane Doe",
    MembershipId = "MEM006",
    Email = "jane.doe@email.com",
    PhoneNumber = "+1-555-0106",
    Address = "123 Library St",
    DateOfBirth = new DateTime(1990, 5, 15),
    MembershipType = MembershipType.Standard
};
var addedPatron = await _patronService.AddPatronAsync(newPatron);
```

### 4. Updating Records

```csharp
// Update a book
var book = await _bookService.GetBookByIdAsync(bookId);
if (book != null)
{
    book.TotalCopies = 10;
    book.AvailableCopies = 8;
    await _bookService.UpdateBookAsync(book);
}

// Update a patron
var patron = await _patronService.GetPatronByIdAsync(patronId);
if (patron != null)
{
    patron.Email = "newemail@example.com";
    patron.PhoneNumber = "+1-555-9999";
    await _patronService.UpdatePatronAsync(patron);
}
```

### 5. Deleting Records

```csharp
// Delete a book (fails if book has active transactions)
var success = await _bookService.DeleteBookAsync(bookId);
if (!success)
{
    // Show error: "Cannot delete book with active transactions"
}

// Delete a patron (fails if patron has active transactions or unpaid fines)
var success = await _patronService.DeletePatronAsync(patronId);
if (!success)
{
    // Show error: "Cannot delete patron with active transactions or unpaid fines"
}
```

### 6. Checkout and Return

```csharp
// Checkout a book (default 14 days)
try
{
    var transaction = await _transactionService.CheckoutBookAsync(
        bookId: 1,
        patronId: 2,
        borrowDays: 14
    );
    // Success!
}
catch (InvalidOperationException ex)
{
    // Handle error: "No copies available" or "Book not found"
}

// Return a book
try
{
    var transaction = await _transactionService.ReturnBookAsync(transactionId);
    
    if (transaction.FineAmount > 0)
    {
        // Show message: "Book returned. Fine applied: $X.XX"
    }
    else
    {
        // Show message: "Book returned successfully"
    }
}
catch (InvalidOperationException ex)
{
    // Handle error: "Transaction not found" or "Book already returned"
}
```

### 7. Fine Management

```csharp
// Get patron's unpaid fines
var unpaidFines = await _fineService.GetPatronUnpaidFinesAsync(patronId);

// Get total unpaid amount
var totalUnpaid = await _fineService.GetTotalUnpaidFinesAsync(patronId);

// Pay a single fine
await _fineService.PayFineAsync(fineId);

// Pay all patron's fines
var success = await _fineService.PayAllPatronFinesAsync(patronId);

// Add a manual fine
var fine = new Fine
{
    PatronId = patronId,
    Amount = 10.00m,
    Reason = "Lost library card replacement fee"
};
await _fineService.AddFineAsync(fine);
```

### 8. Validation

```csharp
// Check if ISBN is unique (when adding)
var isUnique = await _bookService.IsISBNUniqueAsync("978-1234567890");
if (!isUnique)
{
    // Show error: "ISBN already exists"
}

// Check if ISBN is unique (when editing)
var isUnique = await _bookService.IsISBNUniqueAsync("978-1234567890", excludeBookId: currentBookId);

// Check if membership ID is unique
var isUnique = await _patronService.IsMembershipIdUniqueAsync("MEM007");

// Check if email is unique
var isUnique = await _patronService.IsEmailUniqueAsync("test@email.com");
```

### 9. Getting Related Data

```csharp
// Get book with all its transactions
var book = await _bookService.GetBookByIdAsync(bookId);
var transactionHistory = book.Transactions;

// Get patron with transactions and fines
var patron = await _patronService.GetPatronByIdAsync(patronId);
var patronTransactions = patron.Transactions;
var patronFines = patron.Fines;

// Get all transactions for a specific patron
var transactions = await _transactionService.GetPatronTransactionsAsync(patronId);

// Get all transactions for a specific book
var transactions = await _transactionService.GetBookTransactionsAsync(bookId);
```

### 10. Dashboard Statistics

```csharp
// Count active checkouts for a patron
var activeCount = await _transactionService.GetActiveTransactionCountAsync(patronId);

// Check if patron has overdue books
var hasOverdue = await _transactionService.HasOverdueBooks(patronId);

// Get all overdue transactions (for dashboard)
var overdueList = await _transactionService.GetOverdueTransactionsAsync();

// Get all unpaid fines (for dashboard)
var unpaidFines = await _fineService.GetUnpaidFinesAsync();

// Calculate fine amount (for preview)
var fineAmount = _transactionService.CalculateOverdueFine(
    dueDate: transaction.DueDate,
    returnDate: DateTime.UtcNow
);
```

## Error Handling Pattern

```csharp
public async Task CheckoutBook(int bookId, int patronId)
{
    try
    {
        var transaction = await _transactionService.CheckoutBookAsync(bookId, patronId);
        
        // Update UI
        StatusMessage = "Book checked out successfully!";
        await LoadTransactions();
    }
    catch (InvalidOperationException ex)
    {
        // Show user-friendly error
        ErrorMessage = ex.Message;
    }
    catch (Exception ex)
    {
        // Log unexpected errors
        ErrorMessage = "An unexpected error occurred. Please try again.";
        // Log ex for debugging
    }
}
```

## ViewModel Example

```csharp
public class BooksViewModel : ViewModelBase
{
    private readonly BookService _bookService;
    private ObservableCollection<Book> _books;
    private string _searchText;

    public BooksViewModel(BookService bookService)
    {
        _bookService = bookService;
        _books = new ObservableCollection<Book>();
        
        LoadBooksCommand = new RelayCommand(async () => await LoadBooks());
        SearchCommand = new RelayCommand(async () => await SearchBooks());
        AddBookCommand = new RelayCommand(async () => await AddBook());
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set => SetProperty(ref _books, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public ICommand LoadBooksCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddBookCommand { get; }

    private async Task LoadBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        Books = new ObservableCollection<Book>(books);
    }

    private async Task SearchBooks()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await LoadBooks();
        }
        else
        {
            var results = await _bookService.SearchBooksAsync(SearchText);
            Books = new ObservableCollection<Book>(results);
        }
    }

    private async Task AddBook()
    {
        // Show add book dialog
        // After adding:
        await LoadBooks();
    }
}
```

## Important Notes

1. **All service methods are async** - Always use `await` when calling them
2. **Services are scoped** - They're created per request/scope
3. **Transactions are automatic** - EF Core handles database transactions
4. **Navigation properties are loaded** - Use `Include()` for related data
5. **Validation happens in services** - Check return values and catch exceptions
6. **Fine calculation is automatic** - Happens during book return
7. **Available copies update automatically** - On checkout and return

## Testing Services

You can test services directly in a simple console or test method:

```csharp
// In a test or startup method
using var scope = App.AppHost.Services.CreateScope();
var bookService = scope.ServiceProvider.GetRequiredService<BookService>();

var books = await bookService.GetAllBooksAsync();
Console.WriteLine($"Total books: {books.Count}");
```
