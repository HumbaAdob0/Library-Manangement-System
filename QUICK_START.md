# Quick Start Guide - Library Management System

## 🚀 Running the Application

### Option 1: Using Command Line

```bash
# Navigate to project directory
cd "c:\Users\jonrh\OneDrive - ctu.edu.ph\Documents\Project\C#\Library Management System"

# Run the application
dotnet run
```

### Option 2: Using Visual Studio

1. Open `LibraryManagementSystem.csproj` in Visual Studio 2022
2. Press `F5` or click the "Start" button
3. The application will build and launch

## 🔐 Default Login Credentials

- **Username**: `admin`
- **Password**: `admin123`
- **Role**: Administrator

## 📁 Project Files Location

- **Database**: `library.db` (created automatically in project root)
- **Project File**: `LibraryManagementSystem.csproj`
- **Main Entry**: `App.xaml` / `App.xaml.cs`

## ✅ What's Working Now

### 1. Authentication System
- ✅ Login window with Material Design
- ✅ Password validation
- ✅ User authentication with BCrypt
- ✅ Role-based access control
- ✅ Logout functionality

### 2. Main Application Window
- ✅ Navigation menu (Dashboard, Books, Patrons, Transactions, Reports)
- ✅ User information display
- ✅ Material Design theme
- ✅ Responsive layout

### 3. Database & Services
- ✅ SQLite database with Entity Framework Core
- ✅ Complete data models (Book, Patron, Transaction, Fine, User)
- ✅ Service layer for all business logic
- ✅ Dependency injection configured

## 🚧 What's Coming Next

The following features have backend services ready but need UI implementation:

### Phase 2A: Book Management (Next Priority)
- Add new books
- Edit book information
- Delete books
- Search books by title, author, ISBN
- View book availability

### Phase 2B: Patron Management
- Add new patrons
- Edit patron information
- Auto-generate membership IDs
- Search patrons
- View patron history

### Phase 2C: Transaction Management
- Checkout books
- Return books
- Calculate fines automatically
- View active transactions
- View overdue books

### Phase 2D: Reports
- Overdue books report
- Transaction history
- Patron activity
- Export to PDF/Excel

## 🛠️ Development Commands

### Build the Project
```bash
dotnet build
```

### Clean Build Artifacts
```bash
dotnet clean
```

### Restore NuGet Packages
```bash
dotnet restore
```

### Run Without Building
```bash
dotnet run --no-build
```

## 📊 Database Management

### View Database
Use a SQLite browser tool like:
- DB Browser for SQLite (https://sqlitebrowser.org/)
- SQLite Studio (https://sqlitestudio.pl/)

### Reset Database
1. Close the application
2. Delete `library.db` file
3. Restart the application
4. Database will be recreated with default admin user

## 🎨 UI Customization

### Change Theme Colors

Edit `App.xaml`:

```xml
<materialDesign:BundledTheme 
    BaseTheme="Light"           <!-- Light or Dark -->
    PrimaryColor="DeepPurple"   <!-- Primary color -->
    SecondaryColor="Lime" />    <!-- Accent color -->
```

Available colors: Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen, Lime, Yellow, Amber, Orange, DeepOrange, Brown, Grey, BlueGrey

## 🔧 Configuration

### Fine Calculation
Edit `Services/TransactionService.cs`:
```csharp
private const decimal FinePerDay = 5.0m; // Change fine amount
```

### Loan Period
Edit `Services/TransactionService.cs` in `CheckoutBookAsync` method:
```csharp
DueDate = DateTime.Now.AddDays(14) // Change loan period
```

## 📝 Adding New Users

Currently, only the default admin user exists. To add more users:

1. **Option A**: Use a SQLite browser to insert into Users table
2. **Option B**: Implement User Management UI (Phase 3)

Example SQL to add a librarian:
```sql
INSERT INTO Users (Username, PasswordHash, Role, FullName, Email, IsActive, CreatedDate)
VALUES (
    'librarian1',
    '$2a$11$...', -- Use BCrypt to hash password
    'Librarian',
    'John Doe',
    'john@library.com',
    1,
    datetime('now')
);
```

## 🐛 Common Issues

### Issue: Application won't start
**Solution**: 
- Ensure .NET 8.0 SDK is installed
- Run `dotnet restore` and `dotnet build`

### Issue: Database errors
**Solution**: 
- Delete `library.db` and restart
- Check file permissions

### Issue: Material Design icons not showing
**Solution**: 
- Clean and rebuild the project
- Ensure MaterialDesignThemes package is installed

### Issue: Login fails with correct credentials
**Solution**: 
- Delete `library.db` to recreate with default admin
- Check that database was created successfully

## 📚 Next Steps for Development

### To Implement Book Management:

1. Create `Views/BookManagementPage.xaml`
2. Implement `ViewModels/BookManagementViewModel.cs` with:
   - ObservableCollection for books
   - Commands for Add/Edit/Delete
   - Search functionality
3. Update `MainWindow.xaml.cs` to navigate to the page
4. Test CRUD operations

### To Implement Patron Management:

1. Create `Views/PatronManagementPage.xaml`
2. Implement `ViewModels/PatronManagementViewModel.cs`
3. Add navigation in MainWindow
4. Test patron operations

## 💡 Tips

1. **Use Material Design Icons**: Browse available icons at https://materialdesignicons.com/
2. **Follow MVVM Pattern**: Keep business logic in ViewModels, not in code-behind
3. **Use Services**: All database operations should go through services
4. **Test Incrementally**: Test each feature as you build it
5. **Commit Often**: Use git to track your changes

## 📞 Support

If you encounter issues:

1. Check the `README_WPF.md` for detailed documentation
2. Review the `LibraryManagementSystemRequirements.md` for specifications
3. Check build output for specific error messages
4. Ensure all NuGet packages are restored

---

**Ready to start?** Run `dotnet run` and login with `admin` / `admin123`! 🎉
