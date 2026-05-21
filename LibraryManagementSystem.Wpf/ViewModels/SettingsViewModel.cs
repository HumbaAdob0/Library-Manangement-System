using System.Collections.ObjectModel;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly SettingsService _settingsService;
    private readonly GenreService _genreService;
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
    private Genre? _selectedGenre;
    private string _genreName = string.Empty;
    private string _genreStatusMessage = string.Empty;
    private bool _hasGenreStatusMessage;

    public SettingsViewModel(SettingsService settingsService, GenreService genreService)
    {
        _settingsService = settingsService;
        _genreService = genreService;

        var s = _settingsService.Get();
        _libraryName = s.LibraryName;
        _defaultLoanPeriodDays = s.DefaultLoanPeriodDays;
        _finePerDay = s.FinePerDay;

        SaveCommand = new RelayCommand(Save);
        ResetCommand = new RelayCommand(ResetToDefaults);
        SaveGenreCommand = new AsyncRelayCommand(SaveGenreAsync, CanSaveGenre);
        DeleteGenreCommand = new AsyncRelayCommand(DeleteGenreAsync, () => SelectedGenre != null);
        ClearGenreCommand = new RelayCommand(ClearGenreSelection);
    }

    public ObservableCollection<Genre> Genres { get; } = new();

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

    public Genre? SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            if (SetProperty(ref _selectedGenre, value))
            {
                GenreName = value?.Name ?? string.Empty;
                DeleteGenreCommand.RaiseCanExecuteChanged();
                SaveGenreCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(GenreSaveButtonText));
            }
        }
    }

    public string GenreName
    {
        get => _genreName;
        set
        {
            if (SetProperty(ref _genreName, value))
            {
                SaveGenreCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string GenreSaveButtonText => SelectedGenre == null ? "Add Genre" : "Update Genre";

    public string GenreStatusMessage
    {
        get => _genreStatusMessage;
        set
        {
            SetProperty(ref _genreStatusMessage, value);
            HasGenreStatusMessage = !string.IsNullOrWhiteSpace(value);
        }
    }

    public bool HasGenreStatusMessage
    {
        get => _hasGenreStatusMessage;
        set => SetProperty(ref _hasGenreStatusMessage, value);
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
    public AsyncRelayCommand SaveGenreCommand { get; }
    public AsyncRelayCommand DeleteGenreCommand { get; }
    public RelayCommand ClearGenreCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadGenresAsync();
    }

    private async Task LoadGenresAsync()
    {
        try
        {
            var genres = await _genreService.GetAllGenresAsync();
            Genres.Clear();
            foreach (var genre in genres)
            {
                Genres.Add(genre);
            }
        }
        catch (Exception ex)
        {
            GenreStatusMessage = $"Error loading genres: {ex.Message}";
        }
    }

    private void Save()
    {
        var settings = new Settings
        {
            LibraryName = LibraryName,
            DefaultLoanPeriodDays = DefaultLoanPeriodDays,
            FinePerDay = FinePerDay
        };

        _settingsService.Update(settings);

        if (App.Current?.MainWindow != null)
        {
            App.Current.MainWindow.Title = LibraryName;
        }

        StatusMessage = "Settings saved successfully!";

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

        Task.Delay(3000).ContinueWith(_ =>
        {
            App.Current?.Dispatcher.Invoke(() => StatusMessage = string.Empty);
        });
    }

    private bool CanSaveGenre()
    {
        return !string.IsNullOrWhiteSpace(GenreName);
    }

    private async Task SaveGenreAsync()
    {
        var name = GenreName.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            GenreStatusMessage = "Genre name is required.";
            return;
        }

        try
        {
            var isUnique = await _genreService.IsGenreNameUniqueAsync(name, SelectedGenre?.Id);
            if (!isUnique)
            {
                GenreStatusMessage = "A genre with this name already exists.";
                return;
            }

            int? updatedGenreId = null;
            if (SelectedGenre == null)
            {
                await _genreService.AddGenreAsync(new Genre { Name = name });
                GenreStatusMessage = $"Added genre: {name}";
                ClearGenreSelection();
            }
            else
            {
                var selectedId = SelectedGenre.Id;
                await _genreService.UpdateGenreAsync(new Genre
                {
                    Id = selectedId,
                    Name = name,
                    CreatedAt = SelectedGenre.CreatedAt
                });
                GenreStatusMessage = $"Updated genre: {name}";
                updatedGenreId = selectedId;
            }

            await LoadGenresAsync();
            if (updatedGenreId.HasValue)
            {
                SelectedGenre = Genres.FirstOrDefault(g => g.Id == updatedGenreId.Value);
            }
        }
        catch (Exception ex)
        {
            GenreStatusMessage = $"Error saving genre: {ex.Message}";
        }
    }

    private async Task DeleteGenreAsync()
    {
        if (SelectedGenre == null)
        {
            return;
        }

        var genre = SelectedGenre;
        var confirm = MessageBox.Show(
            $"Delete the genre '{genre.Name}'?",
            "Delete Genre",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            await _genreService.DeleteGenreAsync(genre.Id);
            GenreStatusMessage = $"Deleted genre: {genre.Name}";
            ClearGenreSelection();
            await LoadGenresAsync();
        }
        catch (InvalidOperationException ex)
        {
            GenreStatusMessage = ex.Message;
            MessageBox.Show(
                ex.Message,
                "Cannot Delete Genre",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            GenreStatusMessage = $"Error deleting genre: {ex.Message}";
        }
    }

    private void ClearGenreSelection()
    {
        SelectedGenre = null;
        GenreName = string.Empty;
        OnPropertyChanged(nameof(GenreSaveButtonText));
    }
}
