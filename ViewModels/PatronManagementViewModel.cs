using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using LibraryManagementSystem;

namespace LibraryManagementSystem.ViewModels
{
    public class PatronManagementViewModel : ViewModelBase
    {
        private readonly IPatronService _patronService;

        public ObservableCollection<Patron> Patrons { get; } = new ObservableCollection<Patron>();

        private Patron? _selectedPatron;
        public Patron? SelectedPatron
        {
            get => _selectedPatron;
            set => SetProperty(ref _selectedPatron, value);
        }

        private string _searchTerm = string.Empty;
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public PatronManagementViewModel() : this(App.GetService<IPatronService>()) { }

        public PatronManagementViewModel(IPatronService patronService)
        {
            _patronService = patronService;

            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadAllAsync());
            AddCommand = new RelayCommand(async _ => await AddAsync());
            EditCommand = new RelayCommand(_ => { /* Edit handled via SelectedPatron binding */ });
            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());

            _ = LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            Patrons.Clear();
            var list = await _patronService.GetAllPatronsAsync();
            foreach (var p in list) Patrons.Add(p);
        }

        private async Task SearchAsync()
        {
            Patrons.Clear();
            var list = string.IsNullOrWhiteSpace(SearchTerm) ? await _patronService.GetAllPatronsAsync() : await _patronService.SearchPatronsAsync(SearchTerm);
            foreach (var p in list) Patrons.Add(p);
        }

        private async Task AddAsync()
        {
            var newPatron = new Patron
            {
                FullName = "New Patron",
                MembershipId = await _patronService.GenerateMembershipIdAsync(),
                Email = string.Empty,
                PhoneNumber = string.Empty,
                MembershipType = "Standard",
                IsActive = true
            };

            await _patronService.AddPatronAsync(newPatron);
            Patrons.Add(newPatron);
            SelectedPatron = newPatron;
        }

        private async Task SaveAsync()
        {
            if (SelectedPatron == null) return;
            await _patronService.UpdatePatronAsync(SelectedPatron);
            await LoadAllAsync();
        }

        private async Task DeleteAsync()
        {
            if (SelectedPatron == null) return;
            int id = SelectedPatron.Id;
            await _patronService.DeletePatronAsync(id);
            Patrons.Remove(SelectedPatron);
            SelectedPatron = null;
        }
    }
}
