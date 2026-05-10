# Library Management System - Project Structure

## Overview
This is a Windows Forms application built with .NET 10.0 for managing library operations including books and members.

## Project Structure

```
LibraryManagementSystem/
├── Forms/                      # UI Forms (WinForms)
│   ├── MainForm.cs
│   └── MainForm.Designer.cs
│
├── Models/                     # Data models/entities
│   ├── Book.cs
│   └── Member.cs
│
├── Repositories/               # Data access layer
│   ├── IRepository.cs
│   ├── BookRepository.cs
│   └── MemberRepository.cs
│
├── Services/                   # Business logic layer
│   ├── BookService.cs
│   └── MemberService.cs
│
├── Program.cs                  # Application entry point
├── LibraryManagementSystem.csproj
└── LibraryManagementSystemRequirements.md
```

## Architecture Layers

### 1. **Models** (`Models/`)
Contains entity classes representing the core domain objects:
- `Book`: Represents library books with properties like Title, Author, ISBN
- `Member`: Represents library members with contact information

### 2. **Repositories** (`Repositories/`)
Data access layer implementing the Repository pattern:
- `IRepository<T>`: Generic interface for CRUD operations
- `BookRepository`: Book-specific data access
- `MemberRepository`: Member-specific data access

### 3. **Services** (`Services/`)
Business logic layer that orchestrates operations:
- `BookService`: Book-related business logic
- `MemberService`: Member-related business logic

### 4. **Forms** (`Forms/`)
User interface layer using Windows Forms:
- `MainForm`: Main application window

## Technology Stack
- **Framework**: .NET 10.0
- **UI**: Windows Forms
- **Language**: C# with nullable reference types enabled
- **Pattern**: Repository + Service pattern

## Namespace Convention
All namespaces follow the pattern: `LibraryManagementSystem.<FolderName>`

Example:
- `LibraryManagementSystem.Models`
- `LibraryManagementSystem.Repositories`
- `LibraryManagementSystem.Services`
- `LibraryManagementSystem.Forms`

## Building the Project
```bash
dotnet build LibraryManagementSystem.csproj
```

## Running the Project
```bash
dotnet run --project LibraryManagementSystem.csproj
```

## Next Steps
1. Implement database connectivity (SQLite or SQL Server)
2. Complete repository implementations
3. Add additional forms for book and member management
4. Implement borrowing/lending functionality
5. Add search and filtering capabilities
