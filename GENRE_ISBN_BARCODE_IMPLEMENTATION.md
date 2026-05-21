# Genre Dropdown, ISBN-13 Formatting, and Barcode Scanner Implementation Plan

## Overview
This document outlines the implementation of three major features:
1. Genre dropdown with management in Settings
2. ISBN-13 auto-formatting with validation
3. Barcode scanner integration

## Status: IN PROGRESS

### ✅ Completed
1. Created Genre model
2. Updated LibraryDbContext with Genres table
3. Created GenreService with CRUD operations
4. Updated DbSeeder with 20 default genres
5. Registered GenreService in DI container
6. Created ISBNHelper for formatting and validation

### 🔄 In Progress
1. Update BooksViewModel to use GenreService
2. Update BooksView with genre dropdown
3. Add ISBN formatting to BooksView
4. Create SettingsViewModel for genre management
5. Update SettingsView with genre management UI
6. Add barcode scanner integration

### ⏳ Pending
1. Database migration (will happen automatically on first run)
2. Testing all features
3. Build distribution

## Implementation Details

### 1. Genre Dropdown Feature

**Database:**
- ✅ Genre table created with Id, Name, CreatedAt, UpdatedAt
- ✅ Unique constraint on Name
- ✅ 20 default genres seeded

**Services:**
- ✅ GenreService with GetAll, Add, Update, Delete
- ✅ Validation to prevent deleting genres in use

**UI Changes Needed:**
- Update BooksViewModel:
  - Add GenreService dependency
  - Add AvailableGenres ObservableCollection
  - Load genres on initialization
  - Change DialogGenre from string to Genre object
- Update BooksView.xaml:
  - Replace TextBox with ComboBox for genre
  - Bind to AvailableGenres
  - Display Genre.Name

**Settings Management:**
- Create genre management section in SettingsView
- Allow add/edit/delete genres
- Show warning when deleting genre in use

### 2. ISBN-13 Auto-Formatting

**Format:** 978-0-123-45678-9 (13 digits with dashes)

**Helper Class:**
- ✅ ISBNHelper.FormatISBN13() - Formats as user types
- ✅ ISBNHelper.IsValidISBN13() - Validates format and checksum
- ✅ ISBNHelper.GetCursorPosition() - Maintains cursor position

**UI Changes Needed:**
- Update BooksView.xaml:
  - Add TextChanged event handler for ISBN TextBox
  - Format ISBN as user types
  - Show validation error if invalid
- Update BooksViewModel:
  - Add ISBN validation in SaveBookAsync
  - Show error if ISBN is invalid

**Validation Rules:**
- Must be exactly 13 digits
- Must start with 978 or 979
- Must pass checksum validation
- Auto-format with dashes as user types

### 3. Barcode Scanner Integration

**Approach:** Use device camera to scan ISBN barcodes

**Library:** ZXing.Net (popular barcode scanning library for .NET)

**NuGet Packages Needed:**
```
ZXing.Net
ZXing.Net.Bindings.Windows.Compatibility
```

**UI Changes:**
- Add "Scan Barcode" button next to ISBN field
- Open camera dialog
- Scan barcode
- Auto-fill ISBN field with scanned value
- Format and validate scanned ISBN

**Implementation:**
- Create BarcodeScannerWindow
- Use ZXing to access camera
- Decode barcode
- Return ISBN to BooksViewModel

## File Changes Required

### New Files
- ✅ Models/Genre.cs
- ✅ Services/GenreService.cs
- ✅ Helpers/ISBNHelper.cs
- ⏳ Views/BarcodeScannerWindow.xaml
- ⏳ Views/BarcodeScannerWindow.xaml.cs
- ⏳ ViewModels/BarcodeScannerViewModel.cs

### Modified Files
- ✅ Data/LibraryDbContext.cs - Add Genres DbSet
- ✅ Data/DbSeeder.cs - Seed default genres
- ✅ App.xaml.cs - Register GenreService
- ⏳ ViewModels/BooksViewModel.cs - Genre dropdown, ISBN formatting, barcode
- ⏳ Views/BooksView.xaml - UI updates
- ⏳ ViewModels/SettingsViewModel.cs - Genre management
- ⏳ Views/SettingsView.xaml - Genre management UI

## Database Migration

The database will automatically migrate on first run:
1. Genres table will be created
2. Default genres will be seeded
3. Existing books will keep their genre as string (backward compatible)

## Testing Checklist

### Genre Dropdown
- [ ] Load genres in Books dialog
- [ ] Select genre from dropdown
- [ ] Save book with selected genre
- [ ] Edit book and change genre
- [ ] Add new genre in Settings
- [ ] Edit existing genre in Settings
- [ ] Delete unused genre in Settings
- [ ] Try to delete genre in use (should show error)

### ISBN Formatting
- [ ] Type ISBN digits - auto-formats with dashes
- [ ] Paste ISBN - formats correctly
- [ ] Type more than 13 digits - stops at 13
- [ ] Type invalid ISBN - shows validation error
- [ ] Save book with valid ISBN - succeeds
- [ ] Save book with invalid ISBN - shows error

### Barcode Scanner
- [ ] Click "Scan Barcode" button
- [ ] Camera opens
- [ ] Scan book barcode
- [ ] ISBN auto-fills and formats
- [ ] Close camera
- [ ] Handle camera permission denied
- [ ] Handle no camera available

## Next Steps

1. Update BooksViewModel with genre support
2. Update BooksView with genre dropdown and ISBN formatting
3. Create SettingsViewModel with genre management
4. Update SettingsView with genre management UI
5. Add ZXing.Net package
6. Create BarcodeScannerWindow
7. Integrate barcode scanner with BooksView
8. Test all features
9. Build distribution

## Notes

- ISBN-13 is the modern standard (ISBN-10 is deprecated)
- Barcode scanner requires camera permission
- Genre management is admin-only feature
- Existing books will continue to work with string genres
- New books must use dropdown genres
