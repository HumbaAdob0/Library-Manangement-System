using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private readonly IBookService _bookService;

        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set => SetProperty(ref _selectedBook, value);
        }

        private string _searchTerm = string.Empty;
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        public BooksViewModel()
        {
            // Resolve service from App service locator so XAML-instantiated VM works without DI wiring
            _bookService = App.GetService<IBookService>();

            RefreshCommand = new RelayCommand(async _ => await LoadBooksAsync());
            SearchCommand = new RelayCommand(async _ => await SearchBooksAsync());
            AddCommand = new RelayCommand(_ => AddNewBook());
            DeleteCommand = new RelayCommand(async _ => await DeleteSelectedBookAsync(), _ => SelectedBook != null);
            SaveCommand = new RelayCommand(async _ => await SaveSelectedBookAsync(), _ => SelectedBook != null);

            _ = LoadBooksAsync();
        }

        private async Task LoadBooksAsync()
        {
            Books.Clear();
            var items = await _bookService.GetAllBooksAsync();
            foreach (var b in items.OrderBy(b => b.Title))
                Books.Add(b);
        }

        private async Task SearchBooksAsync()
        {
            Books.Clear();
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await LoadBooksAsync();
                return;
            }

            var results = await _bookService.SearchBooksAsync(SearchTerm.Trim());
            foreach (var b in results.OrderBy(b => b.Title))
                Books.Add(b);
        }

        private void AddNewBook()
        {
            var book = new Book
            {
                Title = "New Book",
                Author = string.Empty,
                ISBN = string.Empty,
                Genre = string.Empty,
                Quantity = 1,
                AvailableQuantity = 1,
                CreatedDate = System.DateTime.Now
            };

            Books.Insert(0, book);
            SelectedBook = book;
        }

        private async Task SaveSelectedBookAsync()
        {
            if (SelectedBook == null)
                return;

            try
            {
                if (SelectedBook.Id == 0)
                {
                    await _bookService.AddBookAsync(SelectedBook);
                }
                else
                {
                    await _bookService.UpdateBookAsync(SelectedBook);
                }

                await LoadBooksAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving book: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteSelectedBookAsync()
        {
            if (SelectedBook == null)
                return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedBook.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                await _bookService.DeleteBookAsync(SelectedBook.Id);
                await LoadBooksAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error deleting book: {ex.Message}", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
