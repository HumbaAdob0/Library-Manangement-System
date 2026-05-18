# Backend Configuration - Library Management System

## Overview
The backend has been fully configured using **SQLite** as the database with **Entity Framework Core** for data access, following the MVVM architecture pattern.

## Database Schema

### Tables and Relationships

#### 1. **Users Table**
- `Id` (int, Primary Key)
- `Username` (string, Required, Max 64)
- `UsernameNormalized` (string, Required, Max 64, Unique Index)
- `PasswordHash` (string, Required, Max 512)
- `PasswordSalt` (string, Required, Max 512)
- `Role` (enum: Admin, Librarian)
- `IsActive` (bool)
- `CreatedAt` (DateTime)

#### 2. **Books Table**
- `Id` (int, Primary Key)
- `Title` (string, Required, Max 200)
- `Author` (string, Required, Max 150)
- `ISBN` (string, Required, Max 20, Unique Index)
- `Genre` (string, Max 50)
- `Publisher` (string, Max 150)
- `PublishedYear` (int)
- `TotalCopies` (int)
- `AvailableCopies` (int)
- `Description` (string, Max 1000, Optional)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime, Optional)

#### 3. **Patrons Table**
- `Id` (int, Primary Key)
- `FullName` (string, Required, Max 150)
- `MembershipId` (string, Required, Max 50, Unique Index)
- `Email` (string, Required, Max 100, Unique Index)
- `PhoneNumber` (string, Max 20)
- `Address` (string, Max 300)
- `DateOfBirth` (DateTime)
- `MembershipType` (enum: Standard, Premium)
- `IsActive` (bool)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime, Optional)

#### 4. **Transactions Table**
- `Id` (int, Primary Key)
- `BookId` (int, Foreign Key → Books)
- `PatronId` (int, Foreign Key → Patrons)
- `CheckoutDate` (DateTime)
- `DueDate` (DateTime)
- `ReturnDate` (DateTime, Optional)
- `FineAmount` (decimal(10,2))
- `IsReturned` (bool)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime, Optional)

**Relationships:**
- One Book → Many Transactions
- One Patron → Many Transactions

#### 5. **Fines Table**
- `Id` (int, Primary Key)
- `PatronId` (int, Foreign Key → Patrons)
- `Amount` (decimal(10,2))
- `DateApplied` (DateTime)
- `IsPaid` (bool)
- `DatePaid` (DateTime, Optional)
- `Reason` (string, Max 500, Optional)
- `CreatedAt` (DateTime)

**Relationships:**
- One Patron → Many Fines

## Models Created

All models are located in `LibraryManagementSystem.Wpf/Models/`:

1. **User.cs** - User authentication and authorization
2. **UserRole.cs** - Enum for user roles (Admin, Librarian)
3. **Book.cs** - Book entity with inventory tracking
4. **Patron.cs** - Library member entity
5. **MembershipType.cs** - Enum for membership types (Standard, Premium)
6. **Transaction.cs** - Book checkout/return transactions
7. **Fine.cs** - Overdue fines and payments

## Services Created

All services are located in `LibraryManagementSystem.Wpf/Services/`:

### 1. **BookService.cs**
Handles all book-related operations:
- `GetAllBooksAsync()` - Retrieve all books
- `GetBookByIdAsync(int id)` - Get book details with transactions
- `SearchBooksAsync(string searchTerm)` - Search by title, author, ISBN, or genre
- `GetAvailableBooksAsync()` - Get books with available copies
- `AddBookAsync(Book book)` - Add new book
- `UpdateBookAsync(Book book)` - Update book information
- `DeleteBookAsync(int id)` - Delete book (if no active transactions)
- `IsISBNUniqueAsync(string isbn, int? excludeBookId)` - Validate ISBN uniqueness

### 2. **PatronService.cs**
Handles all patron-related operations:
- `GetAllPatronsAsync()` - Retrieve all patrons
- `GetPatronByIdAsync(int id)` - Get patron with transactions and fines
- `GetPatronByMembershipIdAsync(string membershipId)` - Find by membership ID
- `SearchPatronsAsync(string searchTerm)` - Search by name, membership ID, or email
- `GetActivePatronsAsync()` - Get active patrons only
- `AddPatronAsync(Patron patron)` - Add new patron
- `UpdatePatronAsync(Patron patron)` - Update patron information
- `DeletePatronAsync(int id)` - Delete patron (if no active transactions/unpaid fines)
- `IsMembershipIdUniqueAsync(string membershipId, int? excludePatronId)` - Validate membership ID
- `IsEmailUniqueAsync(string email, int? excludePatronId)` - Validate email uniqueness
- `GetTotalUnpaidFinesAsync(int patronId)` - Calculate total unpaid fines

### 3. **TransactionService.cs**
Handles all transaction operations:
- `GetAllTransactionsAsync()` - Retrieve all transactions
- `GetTransactionByIdAsync(int id)` - Get transaction details
- `GetActiveTransactionsAsync()` - Get unreturned books
- `GetOverdueTransactionsAsync()` - Get overdue books
- `GetPatronTransactionsAsync(int patronId)` - Get patron's transaction history
- `GetBookTransactionsAsync(int bookId)` - Get book's transaction history
- `CheckoutBookAsync(int bookId, int patronId, int borrowDays)` - Checkout book (default 14 days)
- `ReturnBookAsync(int transactionId)` - Return book and calculate fines
- `GetActiveTransactionCountAsync(int patronId)` - Count active checkouts
- `HasOverdueBooks(int patronId)` - Check if patron has overdue books
- `CalculateOverdueFine(DateTime dueDate, DateTime returnDate)` - Calculate fine amount

**Fine Calculation:** $0.50 per day overdue

### 4. **FineService.cs**
Handles all fine-related operations:
- `GetAllFinesAsync()` - Retrieve all fines
- `GetFineByIdAsync(int id)` - Get fine details
- `GetPatronFinesAsync(int patronId)` - Get patron's fine history
- `GetUnpaidFinesAsync()` - Get all unpaid fines
- `GetPatronUnpaidFinesAsync(int patronId)` - Get patron's unpaid fines
- `AddFineAsync(Fine fine)` - Add manual fine
- `PayFineAsync(int fineId)` - Mark fine as paid
- `PayAllPatronFinesAsync(int patronId)` - Pay all patron's fines
- `GetTotalUnpaidFinesAsync(int patronId)` - Calculate total unpaid
- `GetTotalPaidFinesAsync(int patronId)` - Calculate total paid
- `DeleteFineAsync(int id)` - Delete fine record

### 5. **AuthenticationService.cs** (Existing)
Handles user authentication

### 6. **PasswordHasher.cs** (Existing)
Handles password hashing and verification

### 7. **UserSession.cs** (Existing)
Manages current user session

## Database Context

**LibraryDbContext.cs** has been updated with:
- All entity DbSets (Users, Books, Patrons, Transactions, Fines)
- Complete entity configurations with constraints
- Relationship mappings (Foreign Keys, Navigation Properties)
- Indexes for performance optimization
- SQLite-specific configurations

## Database Seeding

**DbSeeder.cs** has been enhanced to seed:
- **2 Users:** admin/Admin@123, librarian/Librarian@123
- **8 Books:** Classic literature and popular titles
- **5 Patrons:** Sample library members
- **2 Active Transactions:** Sample checkouts

## Configuration Files

### 1. **appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=library.db"
  }
}
```

### 2. **LibraryManagementSystem.Wpf.csproj**
Updated packages:
- `Microsoft.EntityFrameworkCore` (8.0.6)
- `Microsoft.EntityFrameworkCore.Sqlite` (8.0.6) ← **Changed from SQL Server**
- `Microsoft.EntityFrameworkCore.Design` (8.0.6)
- `Microsoft.Extensions.Configuration.Json` (8.0.1)
- `Microsoft.Extensions.Hosting` (8.0.1)

### 3. **App.xaml.cs**
Updated to:
- Use `UseSqlite()` instead of `UseSqlServer()`
- Register all new services (BookService, PatronService, TransactionService, FineService)
- Initialize database with `EnsureCreated()` and seed data on startup

## Dependency Injection

All services are registered in `App.xaml.cs`:

```csharp
// Core Services
services.AddSingleton<PasswordHasher>();
services.AddSingleton<UserSession>();
services.AddScoped<AuthenticationService>();
services.AddScoped<DbSeeder>();

// Business Services
services.AddScoped<BookService>();
services.AddScoped<PatronService>();
services.AddScoped<TransactionService>();
services.AddScoped<FineService>();
```

## Database File

The SQLite database file `library.db` will be created automatically in the application's root directory on first run.

## Next Steps

To complete the application, you need to:

1. **Create ViewModels** for each screen:
   - BooksViewModel
   - PatronsViewModel
   - TransactionsViewModel
   - DashboardViewModel
   - ReportsViewModel

2. **Create Views (XAML)** for each screen:
   - BooksView.xaml
   - PatronsView.xaml
   - TransactionsView.xaml
   - DashboardView.xaml
   - ReportsView.xaml

3. **Implement Features:**
   - Book CRUD operations
   - Patron CRUD operations
   - Checkout/Return functionality
   - Fine management
   - Search and filtering
   - Reports generation

4. **Add Validation:**
   - Input validation in ViewModels
   - Business rule validation in Services

5. **Error Handling:**
   - Try-catch blocks in Services
   - User-friendly error messages in ViewModels

## Testing the Backend

To verify the backend is working:

1. Run the application
2. Login with: `admin` / `Admin@123`
3. Check that `library.db` file is created
4. Verify seed data is populated

## Build Status

✅ **Build Successful** - All backend components compile without errors.

⚠️ **Warning:** Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known vulnerability. Consider updating to a newer version when available.
