using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly SettingsService _settingsService;
    private string _libraryName = string.Empty;
    private int _defaultLoanPeriodDays;
    private decimal _finePerDay;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        var s = _settingsService.Get();
        _libraryName = s.LibraryName;
        _defaultLoanPeriodDays = s.DefaultLoanPeriodDays;
        _finePerDay = s.FinePerDay;

        SaveCommand = new RelayCommand(Save);
    }

    public string LibraryName
    {
        get => _libraryName;
        set => SetProperty(ref _libraryName, value);
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

    public RelayCommand SaveCommand { get; }

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
    }
}
