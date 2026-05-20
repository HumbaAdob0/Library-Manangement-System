# Library Management System

A comprehensive Library Management System built with **C# WPF** following the **MVVM (Model-View-ViewModel)** pattern. This application manages library operations including books, patrons, transactions, fines, user management, and reporting with a modern, clean interface.

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/HumbaAdob0/Library-Manangement-System.git
cd '.\Library Management System\LibraryManagementSystem.Wpf\'

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

**Default Login Credentials:**
- **Admin**: Username: `admin` | Password: `Admin@123`
- **Librarian**: Username: `librarian` | Password: `Librarian@123`

## 📋 Prerequisites

Before you begin, ensure you have the following installed on your machine:

### Required Software

1. **[.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)** or later
   - Download and install from Microsoft's official website
   - Verify installation: 
     ```bash
     dotnet --version
     ```
   - Should show version 10.0.x or higher

2. **[Visual Studio Code](https://code.visualstudio.com/)**
   - Download and install from the official website
   - Required extensions:
     - **C# Dev Kit** (Microsoft)
     - **.NET Extension Pack** (Microsoft)
     - **C#** (Microsoft)

3. **[Kiro AI Assistant](https://kiro.ai/)** (Optional but Recommended)
   - AI-powered development assistant for VS Code
   - Helps with code generation, debugging, and refactoring
   - Install from VS Code marketplace or Kiro website

4. **Git** (for version control)
   - [Download Git](https://git-scm.com/downloads)
   - Verify installation: 
     ```bash
     git --version
     ```

### Optional but Recommended

- **[DB Browser for SQLite](https://sqlitebrowser.org/)** - To view and edit the database
- **[Windows Terminal](https://aka.ms/terminal)** - Better terminal experience
- **[GitHub Desktop](https://desktop.github.com/)** - If you prefer GUI for Git

## 🛠️ Setup Instructions

### Step 1: Install Prerequisites

#### 1.1 Install .NET 10.0 SDK

1. Visit [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
2. Download the installer for your operating system (Windows x64 recommended)
3. Run the installer and follow the installation wizard
4. Verify installation by opening a terminal and running:
   ```bash
   dotnet --version
   ```
   You should see output like: `10.0.203` or similar

#### 1.2 Install Visual Studio Code

1. Visit [https://code.visualstudio.com/](https://code.visualstudio.com/)
2. Download the installer for Windows
3. Run the installer with these recommended options:
   - ✅ Add "Open with Code" action to Windows Explorer file context menu
   - ✅ Add "Open with Code" action to Windows Explorer directory context menu
   - ✅ Register Code as an editor for supported file types
   - ✅ Add to PATH

#### 1.3 Install Required VS Code Extensions

Open VS Code and install these extensions:

1. **C# Dev Kit** by Microsoft
   - Press `Ctrl+Shift+X` to open Extensions
   - Search for "C# Dev Kit"
   - Click Install

2. **.NET Extension Pack** by Microsoft
   - Search for ".NET Extension Pack"
   - Click Install

3. **C#** by Microsoft (usually installed automatically with C# Dev Kit)
   - Search for "C#"
   - Click Install if not already installed

#### 1.4 Install Kiro (Optional but Recommended)

1. Visit [https://kiro.ai/](https://kiro.ai/) or search "Kiro" in VS Code Extensions
2. Install the Kiro extension
3. Sign in or create an account
4. Kiro will help you with:
   - Code generation and completion
   - Debugging assistance
   - Code refactoring
   - Documentation generation

#### 1.5 Install Git

1. Visit [https://git-scm.com/downloads](https://git-scm.com/downloads)
2. Download Git for Windows
3. Run the installer with default settings
4. Verify installation:
   ```bash
   git --version
   ```

### Step 2: Clone the Repository

Open a terminal (Windows Terminal, PowerShell, or CMD) and run:

```bash
# Navigate to your desired directory
cd C:\Users\YourUsername\Documents\Projects

# Clone the repository
git clone https://github.com/HumbaAdob0/Library-Manangement-System.git

# Navigate to the project directory
cd "Library Management System\LibraryManagementSystem.Wpf"
```

### Step 3: Open Project in VS Code

```bash
# Open the project in VS Code
code .
```

Or manually:
1. Open VS Code
2. Click `File` → `Open Folder`
3. Navigate to `Library Management System\LibraryManagementSystem.Wpf`
4. Click `Select Folder`

### Step 4: Restore Dependencies

In VS Code, open the integrated terminal (`Ctrl+` ` or View → Terminal) and run:

```bash
# Restore NuGet packages
dotnet restore
```

This will install all required packages:
- Microsoft.EntityFrameworkCore.Sqlite (8.0.0)
- Microsoft.EntityFrameworkCore.Design (8.0.0)
- Microsoft.Extensions.DependencyInjection (8.0.0)
- Microsoft.Extensions.Hosting (8.0.0)
- Microsoft.Extensions.Configuration.Json (8.0.0)
- BCrypt.Net-Next (4.0.3)

### Step 5: Build the Project

```bash
# Build the project
dotnet build
```

You should see output ending with:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Step 6: Run the Application

```bash
# Run the application
dotnet run
```

The application window should open. Use the default credentials to log in:
- **Admin**: `admin` / `Admin@123`
- **Librarian**: `librarian` / `Librarian@123`

### Step 7: Database Setup (Automatic)

The SQLite database (`library.db`) is created automatically on first run with:
- ✅ All required tables (Books, Patrons, Transactions, Fines, Users)
- ✅ Default users (admin and librarian)
- ✅ Sample data (8 books, 5 patrons, 2 transactions)
- ✅ Proper relationships and constraints

**To reset the database:**
```bash
# Stop the application (Ctrl+C in terminal)
# Delete the database file
del library.db

# Restart the application - database will be recreated
dotnet run
```

### Step 8: Verify Installation

After logging in, verify these features work:
1. **📚 Books** - View, search, add, edit, delete books
2. **👥 Patrons** - Manage library members
3. **🔄 Transactions** - Checkout and return books
4. **📊 Reports** - View library statistics and analytics
5. **🔐 Users & Roles** (Admin only) - Manage user accounts
6. **⚙️ Settings** - Application settings (coming soon)

## 📁 Project Structure

```
LibraryManagementSystem.Wpf/
├── Models/                     # Domain entities
│   ├── Book.cs                # Book entity with ISBN, title, author
│   ├── Patron.cs              # Library member entity
│   ├── Transaction.cs         # Checkout/return transactions
│   ├── Fine.cs                # Overdue fines ($0.50/day)
│   ├── User.cs                # System users (Admin/Librarian)
│   ├── UserRole.cs            # User role enumeration
│   └── MembershipType.cs      # Membership type enumeration
│
├── Data/                       # Database layer
│   ├── LibraryDbContext.cs    # EF Core DbContext
│   └── DbSeeder.cs            # Database seeding with sample data
│
├── Services/                   # Business logic layer
│   ├── AuthenticationService.cs  # Login and authentication
│   ├── PasswordHasher.cs         # BCrypt password hashing
│   ├── UserSession.cs            # Current user session
│   ├── BookService.cs            # Book CRUD operations
│   ├── PatronService.cs          # Patron CRUD operations
│   ├── TransactionService.cs     # Transaction management
│   └── FineService.cs            # Fine calculations
│
├── ViewModels/                 # MVVM ViewModels
│   ├── ObservableObject.cs    # Base class with INotifyPropertyChanged
│   ├── RelayCommand.cs        # ICommand implementation
│   ├── AsyncRelayCommand.cs   # Async ICommand implementation
│   ├── LoginViewModel.cs      # Login screen logic
│   ├── MainViewModel.cs       # Main window navigation
│   ├── BooksViewModel.cs      # Books management
│   ├── PatronsViewModel.cs    # Patrons management
│   ├── TransactionsViewModel.cs  # Transactions management
│   ├── UsersViewModel.cs      # User account management (Admin)
│   └── ReportsViewModel.cs    # Reports and analytics
│
├── Views/                      # WPF Views (XAML)
│   ├── LoginWindow.xaml       # Login screen with background image
│   ├── BooksView.xaml         # Books management UI
│   ├── PatronsView.xaml       # Patrons management UI
│   ├── TransactionsView.xaml  # Transactions UI
│   ├── UsersView.xaml         # User management UI (Admin only)
│   └── ReportsView.xaml       # Reports and analytics dashboard
│
├── Converters/                 # Value converters for XAML
│   ├── InverseBooleanToVisibilityConverter.cs
│   └── DaysOverdueConverter.cs
│
├── Helpers/                    # Helper classes
│   └── PasswordBoxHelper.cs   # PasswordBox binding helper
│
├── Assets/                     # Images and resources
│   ├── logo.png               # Application logo
│   └── library_picture.jpg    # Login background image
│
├── App.xaml                    # Application resources and styles
├── App.xaml.cs                 # Application startup & DI configuration
├── MainWindow.xaml             # Main window with sidebar navigation
├── MainWindow.xaml.cs          # Main window code-behind
├── appsettings.json            # Configuration (SQLite connection)
├── library.db                  # SQLite database (auto-generated)
└── LibraryManagementSystem.Wpf.csproj  # Project file
```

## 🎯 Features

### ✅ Completed Features

#### 1. **Authentication System**
- Secure login with BCrypt password hashing
- Role-based access (Admin, Librarian)
- Session management
- Password visibility toggle

#### 2. **Books Management** 📚
- View all books in a searchable data grid
- Add new books (ISBN, Title, Author, Publisher, Year, Copies)
- Edit existing book information
- Delete books
- Search books by title, author, or ISBN
- Track available vs. total copies

#### 3. **Patrons Management** 👥
- View all library members
- Add new patrons (Full Name, Email, Phone, Address, DOB, Membership Type)
- Edit patron information
- Delete patrons
- Search patrons by name or membership ID
- Track membership status (Active/Inactive)
- Membership types: Standard, Premium, Student

#### 4. **Transactions Management** 🔄
- Checkout books to patrons (14-day default period)
- Return books with automatic fine calculation
- View all transactions with filtering:
  - All transactions
  - Active (not returned)
  - Overdue (past due date)
- Automatic copy tracking (decrements on checkout, increments on return)
- Transaction history with dates

#### 5. **Reports & Analytics** 📊
- **Overview Dashboard** with 4 key metrics:
  - Total books (available vs. checked out)
  - Total patrons (active members)
  - Active transactions (overdue count)
  - Total fines (unpaid amount)
- **Most Borrowed Books** (Last 30 days) - Top 10 with borrow counts
- **Top Patrons** (Last 30 days) - Most active library users
- **Overdue Books List** - Real-time with days overdue calculation
- Refresh functionality for latest data

#### 6. **Users & Roles Management** 🔐 (Admin Only)
- View all system users
- Add new users (Username, Password, Role, Active status)
- Edit user information
- Delete users
- Search users by username
- Password hashing for security
- Role assignment (Admin/Librarian)

#### 7. **User Interface**
- Modern, clean design with beige theme (#F6F1E7, #EAD9C7)
- Vertical sidebar navigation with emoji icons
- Responsive layout
- Modal dialogs for add/edit operations
- Status messages and loading indicators
- Logo integration in title bar and sidebar
- Background image on login screen

### 🚧 Planned Features

- **Settings Page** - Application configuration
- **Fine Payment System** - Process fine payments
- **Book Reservations** - Allow patrons to reserve books
- **Email Notifications** - Overdue reminders
- **Export Reports** - PDF/Excel export functionality
- **Advanced Search** - Multi-criteria book search
- **Barcode Scanning** - Quick book checkout/return

## 🏗️ Architecture & Design Patterns

This project follows **MVVM (Model-View-ViewModel)** pattern with clean architecture principles:

### Architecture Layers

1. **Models** - Domain entities and business objects
   - Pure C# classes representing database entities
   - No UI or business logic dependencies

2. **Data** - Entity Framework Core DbContext
   - Database configuration and relationships
   - Automatic migrations and seeding

3. **Services** - Business logic and data operations
   - Encapsulates all database operations
   - Provides clean API for ViewModels
   - Handles transactions and validations

4. **ViewModels** - Presentation logic and data binding
   - Implements INotifyPropertyChanged for data binding
   - Contains UI logic and commands
   - No direct UI element references

5. **Views** - XAML UI components
   - Pure UI markup
   - Data binding to ViewModels
   - No business logic

### Key Design Patterns

- ✅ **MVVM Pattern** - Complete separation of UI and business logic
- ✅ **Dependency Injection** - Microsoft.Extensions.DependencyInjection
- ✅ **Repository Pattern** - Through EF Core DbContext and Services
- ✅ **Command Pattern** - RelayCommand and AsyncRelayCommand for UI actions
- ✅ **Observer Pattern** - INotifyPropertyChanged for reactive UI
- ✅ **Async/Await** - All data operations are asynchronous
- ✅ **Service Layer Pattern** - Business logic separated from data access

### Data Flow

```
User Interaction → View (XAML) → ViewModel (Commands/Properties) 
→ Service Layer → DbContext → SQLite Database
```

## 🧪 Testing the Application

### Manual Testing Checklist

#### 1. **Login System**
```
✅ Login with admin credentials (admin / Admin@123)
✅ Login with librarian credentials (librarian / Librarian@123)
✅ Test password visibility toggle
✅ Test invalid credentials
✅ Verify role-based access (Admin sees Users tab, Librarian doesn't)
```

#### 2. **Books Management**
```
✅ View all books
✅ Search for a book by title
✅ Add a new book
✅ Edit an existing book
✅ Delete a book
✅ Verify copy count updates
```

#### 3. **Patrons Management**
```
✅ View all patrons
✅ Search for a patron
✅ Add a new patron
✅ Edit patron information
✅ Delete a patron
✅ Toggle active/inactive status
```

#### 4. **Transactions**
```
✅ Checkout a book to a patron
✅ Verify available copies decrease
✅ Return a book
✅ Verify available copies increase
✅ Check overdue transactions
✅ Verify fine calculation ($0.50/day)
```

#### 5. **Reports**
```
✅ View overview statistics
✅ Check most borrowed books
✅ Check top patrons
✅ View overdue books list
✅ Verify days overdue calculation
✅ Test refresh functionality
```

#### 6. **Users & Roles (Admin Only)**
```
✅ View all users
✅ Add a new user
✅ Edit user information
✅ Delete a user
✅ Change user role
✅ Toggle active/inactive status
```

### Database Inspection

Use **DB Browser for SQLite** to inspect the database:

1. Open DB Browser for SQLite
2. Click `Open Database`
3. Navigate to `LibraryManagementSystem.Wpf` folder
4. Open `library.db`
5. Explore tables: Books, Patrons, Transactions, Fines, Users

## 🔧 Development with VS Code and Kiro

### Using VS Code for Development

#### Running the Application

1. **Using Terminal**:
   ```bash
   # Open integrated terminal (Ctrl+`)
   dotnet run
   ```

2. **Using Debug**:
   - Press `F5` to start debugging
   - Or click `Run` → `Start Debugging`
   - Set breakpoints by clicking left of line numbers

3. **Hot Reload** (for development):
   ```bash
   dotnet watch run
   ```
   Changes to code will automatically reload the app

#### Building the Project

```bash
# Debug build
dotnet build

# Release build
dotnet build -c Release

# Clean build
dotnet clean
dotnet build
```

#### Useful VS Code Shortcuts

- `Ctrl+Shift+P` - Command Palette
- `Ctrl+` ` - Toggle Terminal
- `Ctrl+Shift+F` - Search in files
- `Ctrl+P` - Quick file open
- `F5` - Start debugging
- `Shift+F5` - Stop debugging
- `F12` - Go to definition
- `Ctrl+.` - Quick fix/refactor

### Using Kiro AI Assistant

Kiro can help you with various development tasks:

#### 1. **Code Generation**
```
Ask Kiro: "Create a new service for managing book categories"
Ask Kiro: "Add a method to calculate late fees for a transaction"
```

#### 2. **Debugging**
```
Ask Kiro: "Why is my book search not working?"
Ask Kiro: "Help me fix this null reference exception"
```

#### 3. **Refactoring**
```
Ask Kiro: "Refactor this method to use async/await"
Ask Kiro: "Extract this code into a separate service"
```

#### 4. **Documentation**
```
Ask Kiro: "Add XML documentation to this class"
Ask Kiro: "Explain what this method does"
```

#### 5. **Testing**
```
Ask Kiro: "Create unit tests for BookService"
Ask Kiro: "Generate test data for the database"
```

### Git Workflow with VS Code

#### Initial Setup

```bash
# Configure Git (first time only)
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

#### Daily Workflow

1. **Check Status**:
   ```bash
   git status
   ```
   Or use VS Code Source Control panel (`Ctrl+Shift+G`)

2. **Create Feature Branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Changes and Commit**:
   ```bash
   # Stage changes
   git add .
   
   # Commit with message
   git commit -m "feat: add book category feature"
   ```
   
   Or use VS Code Source Control panel:
   - Stage files by clicking `+`
   - Enter commit message
   - Click `✓` to commit

4. **Push Changes**:
   ```bash
   git push origin feature/your-feature-name
   ```

#### Commit Message Convention

Use conventional commits format:
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `style:` - Code formatting
- `refactor:` - Code refactoring
- `test:` - Adding tests
- `chore:` - Maintenance tasks

Examples:
```bash
git commit -m "feat: add book search by ISBN"
git commit -m "fix: correct fine calculation for weekends"
git commit -m "docs: update README with setup instructions"
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

### Common Issues and Solutions

#### Issue: "dotnet command not found"
**Solution:**
```bash
# Verify .NET is installed
dotnet --version

# If not found, reinstall .NET 10.0 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/10.0
```

#### Issue: Build errors after cloning
**Solution:**
```bash
# Clean and restore
dotnet clean
dotnet restore
dotnet build
```

#### Issue: "Cannot find library.db"
**Solution:**
```bash
# The database is auto-created on first run
# Just run the application
dotnet run

# If issues persist, delete and recreate:
del library.db
dotnet run
```

#### Issue: NuGet package restore fails
**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# If still failing, check internet connection and NuGet sources
dotnet nuget list source
```

#### Issue: Application won't start
**Solution:**
1. Check if .NET 10.0 SDK is installed: `dotnet --version`
2. Verify you're in the correct directory: `LibraryManagementSystem.Wpf`
3. Check for build errors: `dotnet build`
4. Look at error messages in terminal
5. Try cleaning and rebuilding:
   ```bash
   dotnet clean
   dotnet build
   dotnet run
   ```

#### Issue: Login not working
**Solution:**
- Verify credentials:
  - Admin: `admin` / `Admin@123`
  - Librarian: `librarian` / `Librarian@123`
- Check if database exists: Look for `library.db` file
- Reset database:
  ```bash
  del library.db
  dotnet run
  ```

#### Issue: VS Code C# extension not working
**Solution:**
1. Restart VS Code
2. Reinstall C# Dev Kit extension
3. Check Output panel (`View` → `Output`) for errors
4. Select "C# Dev Kit" from dropdown
5. Reload window: `Ctrl+Shift+P` → "Reload Window"

#### Issue: Changes not reflecting in running app
**Solution:**
```bash
# Stop the application (Ctrl+C)
# Rebuild
dotnet build
# Run again
dotnet run

# Or use hot reload:
dotnet watch run
```

#### Issue: Database locked error
**Solution:**
- Close all instances of the application
- Close DB Browser for SQLite if open
- Restart the application

### Getting Help

If you encounter issues not listed here:

1. **Check the error message** - Read the full error in terminal
2. **Search existing issues** - Check GitHub Issues
3. **Ask Kiro** - Use Kiro AI to help debug
4. **Create an issue** - Provide:
   - Error message
   - Steps to reproduce
   - Your environment (.NET version, OS)
   - Screenshots if applicable

## 📚 Technology Stack

- **Framework**: .NET 10.0 with WPF (Windows Presentation Foundation)
- **Language**: C# 12.0
- **Database**: SQLite with Entity Framework Core 8.0
- **UI Design**: Custom beige theme with modern card-based layout
- **Architecture**: MVVM (Model-View-ViewModel) Pattern
- **Authentication**: BCrypt.Net-Next 4.0.3 for password hashing
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection 8.0.0
- **Configuration**: Microsoft.Extensions.Configuration.Json 8.0.0
- **Development Tools**: 
  - Visual Studio Code
  - Kiro AI Assistant
  - Git for version control

### NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
```

## 🎯 Project Status

### ✅ Completed (Phase 1 & 2)
- ✅ Core infrastructure and architecture
- ✅ Database with EF Core and SQLite
- ✅ Authentication system with role-based access
- ✅ Service layer with business logic
- ✅ MVVM architecture implementation
- ✅ Login window with background image
- ✅ Main window with sidebar navigation
- ✅ Books management (CRUD)
- ✅ Patrons management (CRUD)
- ✅ Transactions management (Checkout/Return)
- ✅ Reports and analytics dashboard
- ✅ Users & Roles management (Admin only)
- ✅ Fine calculation system
- ✅ Sample data seeding

### 🚧 In Progress (Phase 3)
- 🚧 Settings page
- 🚧 Fine payment processing
- 🚧 Advanced search features

### 📋 Planned (Phase 4)
- 📋 Book reservations
- 📋 Email notifications
- 📋 Export reports (PDF/Excel)
- 📋 Barcode scanning
- 📋 Multi-language support
- 📋 Dark mode theme

## 📖 Additional Documentation

- **[README_WPF.md](README_WPF.md)** - Detailed project documentation
- **[QUICK_START.md](QUICK_START.md)** - Quick start guide
- **[CONVERSION_SUMMARY.md](CONVERSION_SUMMARY.md)** - Conversion details
- **[PHASE_1_COMPLETE.md](PHASE_1_COMPLETE.md)** - Phase 1 completion guide

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### For Contributors

1. **Fork the Repository**
   - Click the "Fork" button on GitHub
   - Clone your fork:
     ```bash
     git clone https://github.com/YOUR-USERNAME/Library-Manangement-System.git
     ```

2. **Set Up Development Environment**
   - Follow the setup instructions above
   - Install VS Code and required extensions
   - Install Kiro for AI assistance

3. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Make Your Changes**
   - Follow the coding standards below
   - Write clear, descriptive commit messages
   - Test your changes thoroughly

5. **Commit and Push**
   ```bash
   git add .
   git commit -m "feat: add your feature description"
   git push origin feature/your-feature-name
   ```

6. **Create Pull Request**
   - Go to GitHub and create a Pull Request
   - Describe your changes clearly
   - Link any related issues

### Coding Standards

#### C# Style Guide

```csharp
// ✅ Good
public class BookService
{
    private readonly LibraryDbContext _context;
    
    public BookService(LibraryDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves all books from the database.
    /// </summary>
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }
}

// ❌ Bad
public class bookservice
{
    private LibraryDbContext context;
    
    public List<Book> GetBooks()
    {
        return context.Books.ToList(); // Not async
    }
}
```

**Naming Conventions:**
- **PascalCase**: Classes, methods, properties, public fields
  - `BookService`, `GetAllBooksAsync`, `TotalBooks`
- **camelCase**: Local variables, parameters
  - `bookId`, `patronName`, `searchText`
- **_camelCase**: Private fields
  - `_context`, `_dbContext`, `_isLoading`

**Best Practices:**
- ✅ Use `async/await` for all database operations
- ✅ Add XML documentation comments for public APIs
- ✅ Use nullable reference types (`string?`)
- ✅ Follow MVVM pattern strictly
- ✅ Keep methods small and focused
- ✅ Use meaningful variable names
- ✅ Handle exceptions appropriately

#### XAML Style Guide

```xaml
<!-- ✅ Good -->
<Button x:Name="SaveButton"
        Style="{StaticResource PrimaryButtonStyle}"
        Content="Save"
        Command="{Binding SaveCommand}"
        Width="100"
        Height="36"
        Margin="8,0,0,0" />

<!-- ❌ Bad -->
<Button x:Name="btn1" Content="Save" Command="{Binding SaveCommand}" Width="100" Height="36" Margin="8,0,0,0" />
```

**XAML Conventions:**
- ✅ Use PascalCase for x:Name attributes
- ✅ Organize properties: Name, Style, Content, Binding, Layout
- ✅ Use proper indentation (4 spaces)
- ✅ Use StaticResource for styles
- ✅ Keep XAML clean and readable

### Pull Request Guidelines

**Good PR Title Examples:**
- `feat: add book category management`
- `fix: correct fine calculation for weekends`
- `docs: update setup instructions`
- `refactor: improve transaction service performance`

**PR Description Should Include:**
- What changes were made
- Why the changes were necessary
- How to test the changes
- Screenshots (if UI changes)
- Related issue numbers

### Code Review Process

1. Automated checks will run on your PR
2. Maintainers will review your code
3. Address any feedback or requested changes
4. Once approved, your PR will be merged

### Areas Needing Contribution

- 📋 Settings page implementation
- 📋 Fine payment processing
- 📋 Book reservation system
- 📋 Email notification system
- 📋 Report export (PDF/Excel)
- 📋 Unit tests
- 📋 Documentation improvements
- 📋 Bug fixes

## 📄 License

This project is for educational purposes as part of the Library Management System requirements.

## 👥 Team

**Group 4** - Library Management System Development Team

## 📞 Support

### Getting Help

1. **Check Documentation**
   - Read this README thoroughly
   - Check additional documentation files
   - Review code comments

2. **Use Kiro AI**
   - Ask Kiro for help with code issues
   - Get explanations for error messages
   - Request code examples

3. **Search Issues**
   - Check [GitHub Issues](https://github.com/HumbaAdob0/Library-Manangement-System/issues)
   - Search for similar problems
   - Read closed issues for solutions

4. **Create New Issue**
   - Provide detailed description
   - Include error messages
   - Add steps to reproduce
   - Attach screenshots if relevant

### Contact

For questions or support:
- **GitHub Issues**: [Create an issue](https://github.com/HumbaAdob0/Library-Manangement-System/issues/new)
- **Discussions**: [GitHub Discussions](https://github.com/HumbaAdob0/Library-Manangement-System/discussions)

---

## 🚀 Quick Command Reference

```bash
# Setup
git clone https://github.com/HumbaAdob0/Library-Manangement-System.git
cd "Library Management System/LibraryManagementSystem.Wpf"
dotnet restore
dotnet build

# Development
dotnet run                    # Run application
dotnet watch run             # Run with hot reload
dotnet build                 # Build project
dotnet clean                 # Clean build artifacts

# Database
del library.db               # Delete database (Windows)
dotnet run                   # Recreate database

# Git
git status                   # Check status
git add .                    # Stage all changes
git commit -m "message"      # Commit changes
git push origin branch-name  # Push to remote

# Troubleshooting
dotnet clean                 # Clean project
dotnet restore               # Restore packages
dotnet nuget locals all --clear  # Clear NuGet cache
```

---

**Happy Coding!** 🚀

*Built with ❤️ using C#, WPF, VS Code, and Kiro AI*
