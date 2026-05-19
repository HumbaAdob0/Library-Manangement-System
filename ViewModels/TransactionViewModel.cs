using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using LibraryManagementSystem;

namespace LibraryManagementSystem.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IBookService _bookService;
        private readonly IPatronService _patronService;

        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();

        private Transaction? _selectedTransaction;
        public Transaction? SelectedTransaction
        {
            get => _selectedTransaction;
            set => SetProperty(ref _selectedTransaction, value);
        }

        private string _searchTerm = string.Empty;
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand ReturnCommand { get; }

        public TransactionViewModel() : this(App.GetService<ITransactionService>(), App.GetService<IBookService>(), App.GetService<IPatronService>()) { }

        public TransactionViewModel(ITransactionService transactionService, IBookService bookService, IPatronService patronService)
        {
            _transactionService = transactionService;
            _bookService = bookService;
            _patronService = patronService;

            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadAllAsync());
            CheckoutCommand = new RelayCommand(async _ => await CheckoutAsync());
            ReturnCommand = new RelayCommand(async _ => await ReturnAsync());

            _ = LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            Transactions.Clear();
            var list = await _transactionService.GetAllTransactionsAsync();
            foreach (var t in list) Transactions.Add(t);
        }

        private async Task SearchAsync()
        {
            Transactions.Clear();
            // basic search across patron name or book title
            var list = string.IsNullOrWhiteSpace(SearchTerm) ? await _transactionService.GetAllTransactionsAsync() : (await _transactionService.GetAllTransactionsAsync()).Where(t =>
                t.Patron.FullName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.Book.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

            foreach (var t in list) Transactions.Add(t);
        }

        private async Task CheckoutAsync()
        {
            try
            {
                // Open a simple checkout dialog to choose patron and book
                var dialog = new Views.CheckoutDialog(_bookService, _patronService);
                var showResult = dialog.ShowDialog();
                if (showResult != true)
                    return; // cancelled

                if (!dialog.SelectedBookId.HasValue || !dialog.SelectedPatronId.HasValue)
                {
                    System.Windows.MessageBox.Show("Please select both a book and a patron.", "Checkout", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                var bookId = dialog.SelectedBookId.Value;
                var patronId = dialog.SelectedPatronId.Value;

                var result = await _transactionService.CheckoutBookAsync(bookId, patronId);
                await LoadAllAsync();

                System.Windows.MessageBox.Show($"Checked out '{result.Book?.Title ?? "Book"}' to '{result.Patron?.FullName ?? "Patron"}'.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Checkout Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task ReturnAsync()
        {
            if (SelectedTransaction == null)
                return;

            await _transactionService.ReturnBookAsync(SelectedTransaction.Id);
            await LoadAllAsync();
        }
    }
}
