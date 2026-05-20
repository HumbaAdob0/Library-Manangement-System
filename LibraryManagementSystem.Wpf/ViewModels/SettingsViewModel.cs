using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly SettingsService _settingsService;
    private string _libraryName = string.Empty;
    private string _contactEmail = string.Empty;
    private string _libraryAddress = string.Empty;
    private int _defaultLoanPeriodDays;
    private decimal _finePerDay;
    private int _maxRenewals = 2;
    private int _maxBooksPerPatron = 5;
    private bool _enableEmailNotifications = true;
    private bool _enableAutoRenewal = false;
    private bool _requirePatronVerification = true;
    private string _statusMessage = string.Empty;
    private bool _hasStatusMessage;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        var s = _settingsService.Get();
        _libraryName = s.LibraryName;
        _defaultLoanPeriodDays = s.DefaultLoanPeriodDays;
        _finePerDay = s.FinePerDay;

        SaveCommand = new RelayCommand(Save);
        ResetCommand = new RelayCommand(ResetToDefaults);
    }

    public string LibraryName
    {
        get => _libraryName;
        set => SetProperty(ref _libraryName, value);
    }

    public string ContactEmail
    {
        get => _contactEmail;
        set => SetProperty(ref _contactEmail, value);
    }

    public string LibraryAddress
    {
        get => _libraryAddress;
        set => SetProperty(ref _libraryAddress, value);
    }

    public int DefaultLoanPeriodDays
    {
        get => _defaultLoanPeriodDays;
        set => SetProperty(ref _defaultLoanPeriodDays, value);
    }

    public decimal FinePerDay
    {
        get => _finePerDay;
        set => SetProperty(ref _finePerDay, value);
    }

    public int MaxRenewals
    {
        get => _maxRenewals;
        set => SetProperty(ref _maxRenewals, value);
    }

    public int MaxBooksPerPatron
    {
        get => _maxBooksPerPatron;
        set => SetProperty(ref _maxBooksPerPatron, value);
    }

    public bool EnableEmailNotifications
    {
        get => _enableEmailNotifications;
        set => SetProperty(ref _enableEmailNotifications, value);
    }

    public bool EnableAutoRenewal
    {
        get => _enableAutoRenewal;
        set => SetProperty(ref _enableAutoRenewal, value);
    }

    public bool RequirePatronVerification
    {
        get => _requirePatronVerification;
        set => SetProperty(ref _requirePatronVerification, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            SetProperty(ref _statusMessage, value);
            HasStatusMessage = !string.IsNullOrEmpty(value);
        }
    }

    public bool HasStatusMessage
    {
        get => _hasStatusMessage;
        set => SetProperty(ref _hasStatusMessage, value);
    }

    public RelayCommand SaveCommand { get; }
    public RelayCommand ResetCommand { get; }

    private void Save()
    {
        var settings = new Settings
        {
            LibraryName = LibraryName,
            DefaultLoanPeriodDays = DefaultLoanPeriodDays,
            FinePerDay = FinePerDay
        };

        _settingsService.Update(settings);

        // Apply immediately: broadcast via App-level service or update other services directly as needed.
        // For simplicity, update application title if MainWindow is available
        if (App.Current?.MainWindow != null)
        {
            App.Current.MainWindow.Title = LibraryName;
        }

        StatusMessage = "✓ Settings saved successfully!";
        
        // Clear status message after 3 seconds
        Task.Delay(3000).ContinueWith(_ =>
        {
            App.Current?.Dispatcher.Invoke(() => StatusMessage = string.Empty);
        });
    }

    private void ResetToDefaults()
    {
        LibraryName = "My Library";
        ContactEmail = "library@example.com";
        LibraryAddress = "123 Library Street";
        DefaultLoanPeriodDays = 14;
        FinePerDay = 0.50m;
        MaxRenewals = 2;
        MaxBooksPerPatron = 5;
        EnableEmailNotifications = true;
        EnableAutoRenewal = false;
        RequirePatronVerification = true;

        StatusMessage = "Settings reset to defaults";
        
        // Clear status message after 3 seconds
        Task.Delay(3000).ContinueWith(_ =>
        {
            App.Current?.Dispatcher.Invoke(() => StatusMessage = string.Empty);
        });
    }
}
