using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.ViewModels;

public class UsersViewModel : ObservableObject
{
    private readonly LibraryDbContext _dbContext;
    private readonly PasswordHasher _passwordHasher;
    private ObservableCollection<User> _users;
    private User? _selectedUser;
    private string _searchText = string.Empty;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    // Add/Edit User Properties
    private bool _isAddEditDialogOpen;
    private bool _isEditMode;
    private string _dialogUsername = string.Empty;
    private string _dialogPassword = string.Empty;
    private UserRole _dialogRole = UserRole.Librarian;
    private bool _dialogIsActive = true;

    public UsersViewModel(LibraryDbContext dbContext, PasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _users = new ObservableCollection<User>();

        LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
        SearchCommand = new AsyncRelayCommand(SearchUsersAsync);
        AddUserCommand = new RelayCommand(OpenAddDialog);
        EditUserCommand = new RelayCommand(OpenEditDialog, () => SelectedUser != null);
        DeleteUserCommand = new AsyncRelayCommand(DeleteUserAsync, () => SelectedUser != null);
        SaveUserCommand = new AsyncRelayCommand(SaveUserAsync);
        CancelDialogCommand = new RelayCommand(CloseDialog);
        RefreshCommand = new AsyncRelayCommand(LoadUsersAsync);
    }

    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (SetProperty(ref _selectedUser, value))
            {
                ((RelayCommand)EditUserCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // Dialog Properties
    public bool IsAddEditDialogOpen
    {
        get => _isAddEditDialogOpen;
        set => SetProperty(ref _isAddEditDialogOpen, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public string DialogUsername
    {
        get => _dialogUsername;
        set => SetProperty(ref _dialogUsername, value);
    }

    public string DialogPassword
    {
        get => _dialogPassword;
        set => SetProperty(ref _dialogPassword, value);
    }

    public UserRole DialogRole
    {
        get => _dialogRole;
        set => SetProperty(ref _dialogRole, value);
    }

    public bool DialogIsActive
    {
        get => _dialogIsActive;
        set => SetProperty(ref _dialogIsActive, value);
    }

    public string DialogTitle => IsEditMode ? "Edit User" : "Add New User";

    public ICommand LoadUsersCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand EditUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand SaveUserCommand { get; }
    public ICommand CancelDialogCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading users...";

            var users = await _dbContext.Users
                .OrderBy(u => u.Username)
                .ToListAsync();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            StatusMessage = $"Loaded {Users.Count} users";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading users: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchUsersAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadUsersAsync();
                return;
            }

            StatusMessage = "Searching...";
            var searchTerm = SearchText.ToLower();
            var users = await _dbContext.Users
                .Where(u => u.Username.ToLower().Contains(searchTerm))
                .OrderBy(u => u.Username)
                .ToListAsync();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            StatusMessage = $"Found {Users.Count} users";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error searching: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void OpenAddDialog()
    {
        IsEditMode = false;
        DialogUsername = string.Empty;
        DialogPassword = string.Empty;
        DialogRole = UserRole.Librarian;
        DialogIsActive = true;
        IsAddEditDialogOpen = true;
        OnPropertyChanged(nameof(DialogTitle));
    }

    private void OpenEditDialog()
    {
        if (SelectedUser == null) return;

        IsEditMode = true;
        DialogUsername = SelectedUser.Username;
        DialogPassword = string.Empty; // Don't show existing password
        DialogRole = SelectedUser.Role;
        DialogIsActive = SelectedUser.IsActive;
        IsAddEditDialogOpen = true;
        OnPropertyChanged(nameof(DialogTitle));
    }

    private void CloseDialog()
    {
        IsAddEditDialogOpen = false;
        DialogUsername = string.Empty;
        DialogPassword = string.Empty;
        DialogRole = UserRole.Librarian;
        DialogIsActive = true;
    }

    private async Task SaveUserAsync()
    {
        try
        {
            // Validation
            if (string.IsNullOrWhiteSpace(DialogUsername))
            {
                StatusMessage = "Username is required";
                return;
            }

            if (!IsEditMode && string.IsNullOrWhiteSpace(DialogPassword))
            {
                StatusMessage = "Password is required for new users";
                return;
            }

            if (IsEditMode)
            {
                // Update existing user
                if (SelectedUser == null) return;

                var user = await _dbContext.Users.FindAsync(SelectedUser.Id);
                if (user == null)
                {
                    StatusMessage = "User not found";
                    return;
                }

                user.Username = DialogUsername.Trim();
                user.UsernameNormalized = DialogUsername.Trim().ToUpperInvariant();
                user.Role = DialogRole;
                user.IsActive = DialogIsActive;

                // Update password only if provided
                if (!string.IsNullOrWhiteSpace(DialogPassword))
                {
                    var hashResult = _passwordHasher.HashPassword(DialogPassword);
                    user.PasswordHash = hashResult.Hash;
                    user.PasswordSalt = hashResult.Salt;
                }

                await _dbContext.SaveChangesAsync();
                StatusMessage = $"Updated user: {user.Username}";
            }
            else
            {
                // Check if username already exists
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.UsernameNormalized == DialogUsername.Trim().ToUpperInvariant());

                if (existingUser != null)
                {
                    StatusMessage = "Username already exists";
                    return;
                }

                // Create new user
                var hashResult = _passwordHasher.HashPassword(DialogPassword);
                var newUser = new User
                {
                    Username = DialogUsername.Trim(),
                    UsernameNormalized = DialogUsername.Trim().ToUpperInvariant(),
                    PasswordHash = hashResult.Hash,
                    PasswordSalt = hashResult.Salt,
                    Role = DialogRole,
                    IsActive = DialogIsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                StatusMessage = $"Added user: {newUser.Username}";
            }

            CloseDialog();
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving user: {ex.Message}";
        }
    }

    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null) return;

        try
        {
            var username = SelectedUser.Username;
            var user = await _dbContext.Users.FindAsync(SelectedUser.Id);

            if (user == null)
            {
                StatusMessage = "User not found";
                return;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            StatusMessage = $"Deleted user: {username}";
            SelectedUser = null;
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting user: {ex.Message}";
        }
    }
}
