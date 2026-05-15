using LibraryManagementSystem.Services;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IBookService _bookService;
        private readonly IPatronService _patronService;
        private readonly ITransactionService _transactionService;

        public int TotalBooks { get; private set; }
        public int TotalPatrons { get; private set; }
        public int ActiveCheckouts { get; private set; }
        public int OverdueBooks { get; private set; }

        public DashboardViewModel() : this(App.GetService<IBookService>(), App.GetService<IPatronService>(), App.GetService<ITransactionService>()) { }

        public DashboardViewModel(IBookService bookService, IPatronService patronService, ITransactionService transactionService)
        {
            _bookService = bookService;
            _patronService = patronService;
            _transactionService = transactionService;

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            TotalBooks = books.Count();

            var patrons = await _patronService.GetAllPatronsAsync();
            TotalPatrons = patrons.Count();

            var active = await _transactionService.GetActiveTransactionsAsync();
            ActiveCheckouts = active.Count();

            var overdue = await _transactionService.GetOverdueTransactionsAsync();
            OverdueBooks = overdue.Count();

            OnPropertyChanged(nameof(TotalBooks));
            OnPropertyChanged(nameof(TotalPatrons));
            OnPropertyChanged(nameof(ActiveCheckouts));
            OnPropertyChanged(nameof(OverdueBooks));
        }
    }
}