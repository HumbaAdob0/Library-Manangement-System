# Library Management System

A Windows Forms application for managing library books and members, built with .NET 10 following best practices for layered architecture.

## Project Structure

```
Library Management System/
├── Forms/                          # User Interface Layer
│   ├── MainForm.cs                # Main application window
│   └── MainForm.Designer.cs       # Auto-generated form designer code
├── Models/                         # Domain Models
│   ├── Book.cs                    # Book entity model
│   └── Member.cs                  # Member entity model
├── Services/                       # Business Logic Layer
│   ├── BookService.cs             # Business logic for book operations
│   └── MemberService.cs           # Business logic for member operations
├── Data/                           # Data Access Layer
│   ├── IRepository.cs             # Generic repository interface
│   ├── BookRepository.cs          # Book data repository
│   └── MemberRepository.cs        # Member data repository
├── Program.cs                      # Application entry point
└── Library Management System.csproj  # Project file

```

## Architecture Overview

This project follows a **3-Layer Architecture** pattern:

### 1. **Presentation Layer (Forms)**
- Contains all Windows Forms UI components
- Handles user interactions
- Communicates with the Business Logic layer

### 2. **Business Logic Layer (Services)**
- Contains core business rules and logic
- Validates data
- Orchestrates data operations
- Depends on Data Access layer

### 3. **Data Access Layer (Data)**
- Handles all database operations
- Implements Repository pattern
- Decouples business logic from data sources

## Key Design Patterns

### Repository Pattern
- `IRepository<T>` interface defines data access contracts
- `BookRepository` and `MemberRepository` implement data operations
- Allows for easy testing and switching data sources

### Dependency Injection Ready
- Services accept repositories through constructor injection
- Makes the code testable and maintainable

## Models

### Book
```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

### Member
```csharp
public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; }
}
```

## Services

### BookService
Provides methods for book management:
- `GetAllBooksAsync()` - Retrieve all books
- `GetBookByIdAsync(int id)` - Retrieve specific book
- `AddBookAsync(Book book)` - Add new book
- `UpdateBookAsync(Book book)` - Update book details
- `DeleteBookAsync(int id)` - Delete book

### MemberService
Provides methods for member management:
- `GetAllMembersAsync()` - Retrieve all members
- `GetMemberByIdAsync(int id)` - Retrieve specific member
- `AddMemberAsync(Member member)` - Add new member
- `UpdateMemberAsync(Member member)` - Update member details
- `DeleteMemberAsync(int id)` - Delete member

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/HumbaAdob0/Library-Manangement-System
   ```

2. **Open in Visual Studio**
   - Open `Library Management System.csproj` or the solution file

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## Technology Stack

- **Framework**: .NET 10
- **UI**: Windows Forms
- **Language**: C# 13
- **Nullable Reference Types**: Enabled

## Future Enhancements

- [ ] Database integration (SQL Server / Entity Framework Core)
- [ ] Dependency Injection container
- [ ] Unit tests
- [ ] Logging and error handling
- [ ] Additional forms (Add Book, Add Member, View Books, etc.)
- [ ] Data validation
- [ ] Search and filter functionality

## Best Practices Implemented

✓ **Separation of Concerns** - Code is organized by responsibility
✓ **Single Responsibility Principle** - Each class has one reason to change
✓ **DRY (Don't Repeat Yourself)** - Repository pattern eliminates code duplication
✓ **Dependency Inversion** - Services depend on abstractions (IRepository)
✓ **Async/Await** - All repository operations are asynchronous
✓ **Nullable Reference Types** - Better null safety

## License

[Add your license here]

## Contributing

[Add contribution guidelines here]
