# Library Management System

A comprehensive Library Management System built with **C# WPF** following the **MVVM (Model-View-ViewModel)** pattern. This application manages library operations including books, patrons, transactions, fines, and reporting with a modern Material Design interface.

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/HumbaAdob0/Library-Manangement-System.git
cd "Library Management System"

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

**Default Login:**
- Username: `admin`
- Password: `admin123`

## 📋 Prerequisites

Before you begin, ensure you have the following installed on your machine:

### Required Software

1. **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** or later
   - Download and install from Microsoft's official website
   - Verify installation: `dotnet --version`

2. **IDE (Choose one):**
   - **[Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)** (Recommended)
     - Community Edition (Free) or higher
     - Workloads required:
       - ✅ .NET desktop development
       - ✅ Desktop development with C++
   - **[Visual Studio Code](https://code.visualstudio.com/)** with extensions:
     - C# Dev Kit
     - .NET Extension Pack
   - **[JetBrains Rider](https://www.jetbrains.com/rider/)** (Paid, but excellent)

3. **Git** (for version control)
   - [Download Git](https://git-scm.com/downloads)
   - Verify installation: `git --version`

### Optional but Recommended

- **[DB Browser for SQLite](https://sqlitebrowser.org/)** - To view and edit the database
- **[Windows Terminal](https://aka.ms/terminal)** - Better terminal experience
- **[GitHub Desktop](https://desktop.github.com/)** - If you prefer GUI for Git

## 🛠️ Setup Instructions for Contributors

### 1. Fork and Clone the Repository

```bash
# Fork the repository on GitHub first, then clone your fork
git clone https://github.com/YOUR-USERNAME/Library-Manangement-System.git
cd "Library Management System"

# Add upstream remote to sync with main repository
git remote add upstream https://github.com/HumbaAdob0/Library-Manangement-System.git
```

### 2. Install Dependencies

```bash
# Restore NuGet packages
dotnet restore LibraryManagementSystem.csproj

# This will install:
# - Microsoft.EntityFrameworkCore.Sqlite (8.0.0)
# - Microsoft.EntityFrameworkCore.Design (8.0.0)
# - Microsoft.Extensions.DependencyInjection (8.0.0)
# - Microsoft.Extensions.Hosting (8.0.0)
# - MaterialDesignThemes (5.0.0)
# - BCrypt.Net-Next (4.0.3)
```

### 3. Build the Project

```bash
# Build in Debug mode
dotnet build

# Or build in Release mode
dotnet build -c Release
```

### 4. Run the Application

```bash
# Run from command line
dotnet run

# Or run with hot reload (for development)
dotnet watch run
```

**Using Visual Studio:**
1. Open `LibraryManagementSystem.csproj`
2. Press `F5` to run with debugging
3. Or `Ctrl+F5` to run without debugging

### 5. Database Setup

The SQLite database (`library.db`) is created automatically on first run with:
- ✅ All required tables (Books, Patrons, Transactions, Fines, Users)
- ✅ Default admin user (username: `admin`, password: `admin123`)
- ✅ Proper relationships and constraints

**To reset the database:**
```bash
# Stop the application, then delete the database file
rm library.db  # On Windows: del library.db

# Restart the application - database will be recreated
dotnet run
```

## 📁 Project Structure

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
│   └── [Other ViewModels...]
│
├── Views/                      # WPF Views (XAML)
│   ├── LoginWindow.xaml
│   ├── MainWindow.xaml
│   └── [Other Views...]
│
├── Converters/                 # Value converters for XAML
│   └── BooleanToVisibilityConverter.cs
│
├── App.xaml                    # Application resources
├── App.xaml.cs                 # Application startup & DI
└── LibraryManagementSystem.csproj
```

## 🏗️ Architecture Overview

This project follows **MVVM (Model-View-ViewModel)** pattern with a clean architecture:

### Layers

1. **Models** - Domain entities and business objects
2. **Data** - Entity Framework Core DbContext
3. **Services** - Business logic and data operations
4. **ViewModels** - Presentation logic and data binding
5. **Views** - XAML UI components

### Key Design Patterns

- ✅ **MVVM Pattern** - Separation of UI and business logic
- ✅ **Dependency Injection** - Microsoft.Extensions.DependencyInjection
- ✅ **Repository Pattern** - Through EF Core DbContext
- ✅ **Command Pattern** - RelayCommand for UI actions
- ✅ **Async/Await** - All data operations are asynchronous

## 🧪 Testing

### Manual Testing

1. **Login System**
   ```
   Username: admin
   Password: admin123
   ```

2. **Navigation**
   - Test all menu items (Dashboard, Books, Patrons, Transactions, Reports)

3. **Database Operations**
   - Use DB Browser for SQLite to inspect `library.db`

### Running Tests (When implemented)

```bash
dotnet test
```

## 🔧 Development Workflow

### 1. Create a Feature Branch

```bash
# Update your local main branch
git checkout main
git pull upstream main

# Create a new feature branch
git checkout -b feature/your-feature-name
```

### 2. Make Your Changes

- Follow the existing code style
- Use MVVM pattern for new features
- Add XML documentation comments
- Keep commits atomic and well-described

### 3. Test Your Changes

```bash
# Build to check for errors
dotnet build

# Run the application
dotnet run

# Test your feature thoroughly
```

### 4. Commit Your Changes

```bash
# Stage your changes
git add .

# Commit with a descriptive message
git commit -m "feat: add book search functionality"

# Use conventional commit format:
# feat: new feature
# fix: bug fix
# docs: documentation changes
# style: formatting changes
# refactor: code refactoring
# test: adding tests
# chore: maintenance tasks
```

### 5. Push and Create Pull Request

```bash
# Push to your fork
git push origin feature/your-feature-name

# Go to GitHub and create a Pull Request
```

## 📝 Coding Standards

### C# Style Guide

- Use **PascalCase** for class names, method names, properties
- Use **camelCase** for local variables, parameters
- Use **_camelCase** for private fields
- Add XML documentation for public APIs
- Use `async/await` for asynchronous operations
- Enable nullable reference types

### XAML Style Guide

- Use **PascalCase** for element names
- Use **camelCase** for x:Name attributes
- Organize properties: Name, Layout, Appearance, Behavior
- Use Material Design components when possible

### Example

```csharp
/// <summary>
/// Service for managing book operations.
/// </summary>
public class BookService : IBookService
{
    private readonly LibraryDbContext _context;

    public BookService(LibraryDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all books from the database.
    /// </summary>
    /// <returns>A collection of books.</returns>
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }
}
```

## 🐛 Troubleshooting

### Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Database Issues

```bash
# Delete and recreate database
rm library.db
dotnet run
```

### Package Issues

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Material Design Issues

If icons don't appear:
1. Clean the solution
2. Rebuild the project
3. Restart Visual Studio

## 📚 Technology Stack

- **Framework**: .NET 8.0 with WPF
- **Database**: SQLite with Entity Framework Core 8.0
- **UI Framework**: Material Design In XAML Toolkit 5.0
- **Architecture**: MVVM Pattern
- **Authentication**: BCrypt.Net for password hashing
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection

## 🎯 Current Status

### ✅ Phase 1 Complete
- Core infrastructure
- Database with EF Core
- Authentication system
- Service layer
- MVVM architecture
- Login and main window

### 🚧 Phase 2 In Progress
- Book management UI
- Patron management UI
- Transaction processing UI
- Dashboard
- Reports

## 📖 Additional Documentation

- **[README_WPF.md](README_WPF.md)** - Detailed project documentation
- **[QUICK_START.md](QUICK_START.md)** - Quick start guide
- **[CONVERSION_SUMMARY.md](CONVERSION_SUMMARY.md)** - Conversion details
- **[PHASE_1_COMPLETE.md](PHASE_1_COMPLETE.md)** - Phase 1 completion guide

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'feat: add some amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Contribution Guidelines

- Follow the existing code style
- Write clear commit messages
- Update documentation as needed
- Test your changes thoroughly
- Keep PRs focused on a single feature/fix

## 📄 License

This project is for educational purposes as part of the Library Management System requirements.

## 👥 Team

**Group 4** - Library Management System Development Team

## 📞 Support

For questions or issues:
1. Check the documentation files
2. Search existing GitHub issues
3. Create a new issue with detailed information

---

**Happy Coding!** 🚀
