using System.Collections.ObjectModel;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class PatronsViewModel : ObservableObject
{
    private readonly PatronService _patronService;
    private ObservableCollection<Patron> _patrons;
    private Patron? _selectedPatron;
    private string _searchText = string.Empty;
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    public PatronsViewModel(PatronService patronService)
    {
        _patronService = patronService;
        _patrons = new ObservableCollection<Patron>();

        LoadPatronsCommand = new AsyncRelayCommand(LoadPatronsAsync);
        SearchCommand = new AsyncRelayCommand(SearchPatronsAsync);
        AddPatronCommand = new RelayCommand(OpenAddDialog);
        EditPatronCommand = new RelayCommand(OpenEditDialog, () => SelectedPatron != null);
        DeletePatronCommand = new AsyncRelayCommand(DeletePatronAsync, () => SelectedPatron != null);
        RefreshCommand = new AsyncRelayCommand(LoadPatronsAsync);
    }

    public ObservableCollection<Patron> Patrons
    {
        get => _patrons;
        set => SetProperty(ref _patrons, value);
    }

    public Patron? SelectedPatron
    {
        get => _selectedPatron;
        set
        {
            if (SetProperty(ref _selectedPatron, value))
            {
                ((RelayCommand)EditPatronCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)DeletePatronCommand).RaiseCanExecuteChanged();
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

    public ICommand LoadPatronsCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddPatronCommand { get; }
    public ICommand EditPatronCommand { get; }
    public ICommand DeletePatronCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadPatronsAsync();
    }

    private async Task LoadPatronsAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading patrons...";

            var patrons = await _patronService.GetAllPatronsAsync();
            Patrons.Clear();
            foreach (var patron in patrons)
            {
                Patrons.Add(patron);
            }

            StatusMessage = $"Loaded {Patrons.Count} patrons";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading patrons: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchPatronsAsync()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadPatronsAsync();
                return;
            }

            StatusMessage = "Searching...";
            var patrons = await _patronService.SearchPatronsAsync(SearchText);
            Patrons.Clear();
            foreach (var patron in patrons)
            {
                Patrons.Add(patron);
            }

            StatusMessage = $"Found {Patrons.Count} patrons";
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

    private void AddPatron()
    {
        OpenAddDialog();
    }

    private void EditPatron()
    {
        if (SelectedPatron == null) return;
        OpenEditDialog();
    }

    // Dialog properties
    private bool _isDialogOpen;
    private bool _isEditMode;
    private string _dialogFullName = string.Empty;
    private string _dialogMembershipId = string.Empty;
    private string _dialogEmail = string.Empty;
    private string _dialogPhone = string.Empty;
    private MembershipType _dialogMembershipType = MembershipType.Standard;

    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set => SetProperty(ref _isDialogOpen, value);
    }

    public bool IsDialogEditMode
    {
        get => _isEditMode;
        set
        {
            if (SetProperty(ref _isEditMode, value))
            {
                OnPropertyChanged(nameof(DialogTitle));
            }
        }
    }

    public string DialogTitle => IsDialogEditMode ? "Edit Patron" : "Add Patron";

    public string DialogFullName { get => _dialogFullName; set => SetProperty(ref _dialogFullName, value); }
    public string DialogMembershipId { get => _dialogMembershipId; set => SetProperty(ref _dialogMembershipId, value); }
    public string DialogEmail { get => _dialogEmail; set => SetProperty(ref _dialogEmail, value); }
    public string DialogPhone { get => _dialogPhone; set => SetProperty(ref _dialogPhone, value); }
    public MembershipType DialogMembershipType { get => _dialogMembershipType; set => SetProperty(ref _dialogMembershipType, value); }

    public ICommand SaveDialogCommand => new AsyncRelayCommand(SaveDialogAsync);
    public ICommand CancelDialogCommand => new RelayCommand(CloseDialog);

    private void OpenAddDialog()
    {
        IsDialogEditMode = false;
        DialogFullName = string.Empty;
        DialogMembershipId = string.Empty;
        DialogEmail = string.Empty;
        DialogPhone = string.Empty;
        DialogMembershipType = MembershipType.Standard;
        IsDialogOpen = true;
    }

    private void OpenEditDialog()
    {
        if (SelectedPatron == null) return;

        IsDialogEditMode = true;
        DialogFullName = SelectedPatron.FullName;
        DialogMembershipId = SelectedPatron.MembershipId;
        DialogEmail = SelectedPatron.Email;
        DialogPhone = SelectedPatron.PhoneNumber;
        DialogMembershipType = SelectedPatron.MembershipType;
        IsDialogOpen = true;
    }

    private void CloseDialog()
    {
        IsDialogOpen = false;
    }

    private async Task SaveDialogAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(DialogFullName))
            {
                StatusMessage = "Full name is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(DialogMembershipId))
            {
                StatusMessage = "Membership ID is required";
                return;
            }

            if (IsDialogEditMode)
            {
                if (SelectedPatron == null) return;

                var patron = new Patron
                {
                    Id = SelectedPatron.Id,
                    FullName = DialogFullName.Trim(),
                    MembershipId = DialogMembershipId.Trim(),
                    Email = DialogEmail.Trim(),
                    PhoneNumber = DialogPhone.Trim(),
                    MembershipType = DialogMembershipType,
                    CreatedAt = SelectedPatron.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = SelectedPatron.IsActive
                };

                var result = await _patronService.UpdatePatronAsync(patron);
                if (result != null)
                {
                    // replace in collection
                    var existingPatron = Patrons.FirstOrDefault(p => p.Id == result.Id);
                    if (existingPatron != null)
                    {
                        var idx = Patrons.IndexOf(existingPatron);
                        if (idx >= 0) Patrons[idx] = result;
                    }
                    SelectedPatron = result;
                    StatusMessage = $"Updated patron: {result.FullName}";
                    CloseDialog();
                }
                else
                {
                    StatusMessage = "Failed to update patron";
                }
            }
            else
            {
                var patron = new Patron
                {
                    FullName = DialogFullName.Trim(),
                    MembershipId = DialogMembershipId.Trim(),
                    Email = DialogEmail.Trim(),
                    PhoneNumber = DialogPhone.Trim(),
                    MembershipType = DialogMembershipType,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _patronService.AddPatronAsync(patron);
                if (result != null)
                {
                    Patrons.Add(result);
                    StatusMessage = $"Added patron: {result.FullName}";
                    CloseDialog();
                }
                else
                {
                    StatusMessage = "Failed to add patron";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving patron: {ex.Message}";
        }
    }

    private async Task DeletePatronAsync()
    {
        if (SelectedPatron == null) return;

        try
        {
            var patronName = SelectedPatron.FullName;
            var success = await _patronService.DeletePatronAsync(SelectedPatron.Id);

            if (success)
            {
                Patrons.Remove(SelectedPatron);
                StatusMessage = $"Deleted: {patronName}";
                SelectedPatron = null;
            }
            else
            {
                StatusMessage = "Cannot delete patron with active transactions or unpaid fines";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting patron: {ex.Message}";
        }
    }
}
