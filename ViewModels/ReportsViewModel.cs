using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly ITransactionService _transactionService;

        public ObservableCollection<Transaction> OverdueTransactions { get; } = new ObservableCollection<Transaction>();
        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();

        public ReportsViewModel() : this(App.GetService<ITransactionService>()) { }

        public ReportsViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            OverdueTransactions.Clear();
            var overdue = await _transactionService.GetOverdueTransactionsAsync();
            foreach (var t in overdue) OverdueTransactions.Add(t);

            Transactions.Clear();
            var all = await _transactionService.GetAllTransactionsAsync();
            foreach (var t in all) Transactions.Add(t);
        }
    }
}
