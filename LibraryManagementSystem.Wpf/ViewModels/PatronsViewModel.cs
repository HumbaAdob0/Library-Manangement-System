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
        AddPatronCommand = new RelayCommand(AddPatron);
        EditPatronCommand = new RelayCommand(EditPatron, () => SelectedPatron != null);
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
        StatusMessage = "Add patron feature - Coming soon";
        // TODO: Open add patron dialog
    }

    private void EditPatron()
    {
        if (SelectedPatron == null) return;
        StatusMessage = $"Edit patron feature - Coming soon (Selected: {SelectedPatron.FullName})";
        // TODO: Open edit patron dialog
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
