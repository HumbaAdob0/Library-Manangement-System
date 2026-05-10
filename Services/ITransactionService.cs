using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CheckoutBookAsync(int bookId, int patronId, int loanDays = 14);
        Task<Transaction> ReturnBookAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetTransactionsByPatronAsync(int patronId);
        Task<IEnumerable<Transaction>> GetOverdueTransactionsAsync();
        Task<IEnumerable<Transaction>> GetActiveTransactionsAsync();
        Task<decimal> CalculateFineAsync(int transactionId);
    }
}
