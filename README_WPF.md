# Library Management System - WPF Application

## Project Overview

A comprehensive Library Management System built with **C# WPF** following the **MVVM (Model-View-ViewModel)** pattern. This application manages library operations including books, patrons, transactions, fines, and reporting.

## Technology Stack

- **Framework**: .NET 8.0 with WPF
- **Database**: SQLite with Entity Framework Core
- **UI Framework**: Material Design In XAML Toolkit
- **Architecture**: MVVM Pattern
- **Authentication**: BCrypt.Net for password hashing
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection

## Project Structure

```
LibraryManagementSystem/
├── Models/                     # Domain entities
│   ├── Book.cs                # Book entity
│   ├── Patron.cs              # Library member entity
│   ├── Transaction.cs         # Checkout/return transactions
│   ├── Fine.cs                # Overdue fines
│   └── User.cs                # System users (Admin/Librarian)
│
├── Data/                       # Database context
│   └── LibraryDbContext.cs    # EF Core DbContext
│
├── Services/                   # Business logic layer
│   ├── IAuthenticationService.cs
│   ├── AuthenticationService.cs
│   ├── IBookService.cs
│   ├── BookService.cs
│   ├── IPatronService.cs
│   ├── PatronService.cs
│   ├── ITransactionService.cs
│   ├── TransactionService.cs
│   ├── IFineService.cs
│   └── FineService.cs
│
├── ViewModels/                 # MVVM ViewModels
│   ├── ViewModelBase.cs       # Base class for all ViewModels
│   ├── RelayCommand.cs        # ICommand implementation
│   ├── LoginViewModel.cs
│   ├── MainViewModel.cs
│   ├── BookManagementViewModel.cs
│   ├── PatronManagementViewModel.cs
│   ├── TransactionViewModel.cs
│   └── ReportsViewModel.cs
│
├── Views/                      # WPF Views (XAML)
│   ├── LoginWindow.xaml
│   ├── LoginWindow.xaml.cs
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
│
├── Converters/                 # Value converters for XAML binding
│   └── BooleanToVisibilityConverter.cs
│
├── App.xaml                    # Application resources
├── App.xaml.cs                 # Application startup & DI configuration
└── LibraryManagementSystem.csproj
```

## Database Schema

### Tables

1. **Books**
   - Id, Title, Author, ISBN, Genre, Publisher, PublishedYear
   - Quantity, AvailableQuantity, CreatedDate

2. **Patrons**
   - Id, MembershipId, FullName, Email, PhoneNumber, Address
   - DateOfBirth, MembershipType, IsActive, JoinDate

3. **Transactions**
   - Id, BookId, PatronId, CheckoutDate, DueDate, ReturnDate
   - FineAmount, Status

4. **Fines**
   - Id, PatronId, Amount, DateApplied, IsPaid, Reason

5. **Users**
   - Id, Username, PasswordHash, Role, FullName, Email
   - IsActive, CreatedDate

## Features Implemented

### ✅ Phase 1 - Core Infrastructure (COMPLETED)

1. **Database Setup**
   - SQLite database with Entity Framework Core
   - Complete schema with relationships
   - Seed data (default admin user)

2. **Authentication System**
   - Login/Logout functionality
   - Password hashing with BCrypt
   - Role-based access (Admin/Librarian)
   - Default credentials: `admin` / `admin123`

3. **Service Layer**
   - Book management service
   - Patron management service
   - Transaction management service
   - Fine management service
   - Authentication service

4. **MVVM Architecture**
   - Base ViewModel with INotifyPropertyChanged
   - RelayCommand for command binding
   - Dependency injection setup
   - Service registration

5. **UI Framework**
   - Material Design theme
   - Login window with validation
   - Main window with navigation menu
   - Responsive layout

### 🚧 Phase 2 - Feature Implementation (IN PROGRESS)

The following features have service layer implementations but need UI:

1. **Book Management**
   - Add/Edit/Delete books
   - Search and filter books
   - Track availability

2. **Patron Management**
   - Add/Edit/Delete patrons
   - Auto-generate membership IDs
   - Search patrons

3. **Transaction Management**
   - Checkout books
   - Return books
   - Calculate overdue fines
   - View transaction history

4. **Fine Management**
   - Track unpaid fines
   - Mark fines as paid
   - View fine history

5. **Reports & Analytics**
   - Overdue books report
   - Checked out books report
   - Transaction history report
   - Patron activity report

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Windows OS (for WPF)

### Installation

1. **Clone or navigate to the project directory**

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

### First Time Setup

1. The application will automatically create the SQLite database (`library.db`) on first run
2. A default admin user is created:
   - **Username**: `admin`
   - **Password**: `admin123`
3. Login with these credentials to access the system

## Configuration

### Database Connection

The database connection string is configured in `App.xaml.cs`:

```csharp
services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));
```

To change the database location, modify the connection string.

### Fine Calculation

The fine per day for overdue books is set in `TransactionService.cs`:

```csharp
private const decimal FinePerDay = 5.0m; // $5 per day overdue
```

### Loan Period

Default loan period is 14 days, configurable in the `CheckoutBookAsync` method.

## User Roles

### Admin
- Full system access
- Manage users
- Manage books, patrons, transactions
- View all reports
- System settings

### Librarian
- Manage books
- Manage patrons
- Process checkouts/returns
- View reports
- Cannot manage users or system settings

## Development Roadmap

### Phase 2: UI Implementation (Next Steps)

1. **Dashboard Page**
   - Statistics cards (total books, active patrons, overdue books)
   - Recent transactions
   - Quick actions

2. **Book Management Page**
   - DataGrid with book list
   - Add/Edit book dialog
   - Search and filter functionality
   - Book availability indicator

3. **Patron Management Page**
   - DataGrid with patron list
   - Add/Edit patron dialog
   - Search functionality
   - View patron borrowing history

4. **Transaction Page**
   - Checkout interface
   - Return interface
   - Active transactions list
   - Overdue transactions alert

5. **Reports Page**
   - Report selection
   - Date range filters
   - Export to PDF/Excel functionality
   - Print preview

### Phase 3: Advanced Features

1. **User Management** (Admin only)
   - Add/Edit/Delete users
   - Reset passwords
   - Manage roles

2. **Advanced Search**
   - Multi-criteria search
   - Saved searches
   - Search history

3. **Notifications**
   - Overdue reminders
   - Due date notifications
   - System alerts

4. **Backup & Restore**
   - Database backup
   - Restore from backup
   - Export data

## Testing

### Manual Testing

1. **Login**
   - Test with valid credentials
   - Test with invalid credentials
   - Test logout functionality

2. **Navigation**
   - Test all menu items
   - Verify role-based access

### Unit Testing (To be implemented)

- Service layer unit tests
- ViewModel unit tests
- Business logic validation

## Troubleshooting

### Database Issues

If you encounter database errors:

1. Delete the `library.db` file
2. Restart the application
3. The database will be recreated automatically

### Build Errors

If you get build errors:

1. Clean the solution: `dotnet clean`
2. Restore packages: `dotnet restore`
3. Rebuild: `dotnet build`

### Material Design Theme Issues

If Material Design icons don't appear:

1. Ensure MaterialDesignThemes package is installed
2. Check that App.xaml includes Material Design resources
3. Rebuild the project

## Contributing

When adding new features:

1. Follow the MVVM pattern
2. Create services for business logic
3. Use dependency injection
4. Add appropriate error handling
5. Update this README

## License

This project is for educational purposes as part of the Library Management System requirements.

## Contact

For questions or issues, contact the development team.

---

**Status**: Phase 1 Complete ✅ | Phase 2 In Progress 🚧

**Last Updated**: 2024
